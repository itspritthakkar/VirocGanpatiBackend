using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;
using VirocGanpati.Data;
using VirocGanpati.DTOs;
using VirocGanpati.DTOs.Payments;
using VirocGanpati.Enums;
using VirocGanpati.Helpers;
using VirocGanpati.Models;
using VirocGanpati.Services;

namespace VirocGanpati.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMandalService _mandalService;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public PaymentController(IUserService userService, IRoleService roleService, IMandalService mandalService, ApplicationDbContext context, IConfiguration config)
        {
            _userService = userService;
            _roleService = roleService;
            _mandalService = mandalService;
            _context = context;
            _config = config;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreatePaymentRequestDto request)
        {
            try
            {
                bool emailExists = await _userService.CheckIfEmailExists(request.Email);
                if (emailExists) throw new InvalidDataException("Email already exists in our records");

                bool mobileExists = await _userService.CheckIfMobileExists(request.Mobile);
                if (mobileExists) throw new InvalidDataException("Mobile number already exists in our records");

                var client = new RazorpayClient(_config["Razorpay:Key"], _config["Razorpay:Secret"]);

                int Amount = 100;

                var options = new Dictionary<string, object>
                                {
                                    { "amount", Amount * 100 }, // Amount in paise
                                    { "currency", "INR" },
                                    { "receipt", Guid.NewGuid().ToString() },
                                    { "payment_capture", 1 }
                                };

                Order order = client.Order.Create(options);

                var payment = new Models.Payment
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Mobile = request.Mobile,
                    Amount = Amount,
                    MandalName = request.MandalName,
                    MandalDescription = request.MandalDescription,
                    AreaId = request.AreaId,
                    ArtiMorningTimeId = request.ArtiMorningTimeId,
                    ArtiEveningTimeId = request.ArtiEveningTimeId,
                    DateOfVisarjanId = request.DateOfVisarjanId,
                    RazorpayOrderId = order["id"].ToString(),
                    Status = PaymentStatus.Initiated
                };

                _context.Payments.Add(payment);
                _context.SaveChanges();

                return Ok(new
                {
                    orderId = order["id"].ToString(),
                    key = _config["Razorpay:Key"],
                    amount = Amount * 100,
                    currency = "INR",
                    paymentId = payment.PaymentId
                });
            } catch (InvalidDataException e)
            {
                return ResponseHelper.BadRequestMessage(e.Message);
            }


        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] RazorpayVerificationRequestDto request)
        {
            try
            {
                var generatedSignature = GenerateSignature(request.RazorpayOrderId, request.RazorpayPaymentId);

                Models.Payment payment = _context.Payments.FirstOrDefault(p => p.RazorpayOrderId == request.RazorpayOrderId) ?? throw new InvalidDataException("Invalid Razorpay Order Id");

                UserDto? userDto = null;
                string? token = null;
                if (generatedSignature == request.RazorpaySignature)
                {
                    payment.RazorpayPaymentId = request.RazorpayPaymentId;
                    payment.RazorpaySignature = request.RazorpaySignature;
                    payment.Status = PaymentStatus.Success;

                    RoleDto role = await _roleService.GetRoleByIdentifierAsync("User") ?? throw new InvalidDataException("Roles not populated properly");

                    AddMandalDto addMandalDto = new()
                    {
                        MandalName = payment.MandalName,
                        MandalDescription = payment.MandalDescription,
                        Status = "Active",
                        AreaId = payment.AreaId,
                        ArtiMorningTimeId = payment.ArtiMorningTimeId,
                        ArtiEveningTimeId = payment.ArtiEveningTimeId,
                        DateOfVisarjanId = payment.DateOfVisarjanId,
                        CreatedBy = payment.FirstName + " " + payment.LastName
                    };
                    MandalDto mandalDto = await _mandalService.AddMandalAsync(addMandalDto);

                    AddUserDto addUserDto = new()
                    {
                        FirstName = payment.FirstName,
                        LastName = payment.LastName,
                        Email = payment.Email,
                        Mobile = payment.Mobile,
                        Password = request.Password,
                        RoleId = role.RoleId,
                        MandalId = mandalDto.MandalId,
                        Status = "Active"
                    };
                    userDto = await _userService.AddUserAsync(addUserDto);
                    User user = await _userService.ValidateUserAsync(payment.Mobile, request.Password);

                    token = JwtTokenHelper.GenerateJwtToken(user, _config);
                }
                else
                {
                    payment.Status = PaymentStatus.Failed;
                }

                _context.SaveChanges();

                return Ok(new { status = payment.Status.ToString(), user = userDto, token });
            }
            catch (InvalidDataException e)
            {
                return ResponseHelper.BadRequestMessage(e.Message);
            }

        }

        private string GenerateSignature(string orderId, string paymentId)
        {
            var secret = _config["Razorpay:Secret"];
            var payload = $"{orderId}|{paymentId}";
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
