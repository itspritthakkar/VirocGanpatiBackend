using VirocGanpati.DTOs.Otp;
using VirocGanpati.Exceptions;
using VirocGanpati.Models;
using VirocGanpati.Repositories;

namespace VirocGanpati.Services
{
    public class OtpService : IOtpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IOtpRepository _otpRepository;

        public OtpService(IHttpClientFactory httpClientFactory, IConfiguration config, IOtpRepository otpRepository)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
            _otpRepository = otpRepository;
        }

        public async Task<OtpMessage> SendOtpAsync(string mobile, string purpose, int otpSize, string firstName)
        {
            var existingOtp = await _otpRepository.GetActiveOtpAsync(mobile, purpose);
            if (existingOtp != null && existingOtp.ExpiryAt > DateTime.UtcNow)
            {
                throw new OtpAlreadyExistsException("At OTP already exists");
            }

            return await CreateAndSendOtpAsync(mobile, purpose, otpSize, firstName);
        }

        public async Task<OtpMessage> ResendOtpAsync(string mobile, string purpose, int otpSize, string firstName)
        {
            var existingOtp = await _otpRepository.GetActiveOtpAsync(mobile, purpose);
            if (existingOtp != null)
            {
                existingOtp.ExpiryAt = DateTime.UtcNow; // Expire immediately
                await _otpRepository.UpdateOtpAsync(existingOtp);
            }

            return await CreateAndSendOtpAsync(mobile, purpose, otpSize, firstName);
        }

        private async Task<OtpMessage> CreateAndSendOtpAsync(string mobile, string purpose, int otpSize, string firstName)
        {
            // Generate OTP
            string otpCode = GenerateOtp(otpSize);

            // Prepare SMS payload
            var payload = new
            {
                APIKey = _config["SMSLane:ApiKey"],
                ClientId = _config["SMSLane:ClientId"],
                SenderId = _config["SMSLane:SenderId"],
                Is_Unicode = true,
                Is_Flash = false,
                Message = $"Namaste, {firstName}, your India થી ભારત 2025 (VIROC Ganesh Utsav Competition) registration is successful. Your code is {otpCode}.",
                MobileNumbers = $"91{mobile}"
            };

            // Call SMS API
            var url = _config["SMSLane:SendSmsUrl"];
            var response = await _httpClient.PostAsJsonAsync(url, payload);
            var smsLaneResult = await response.Content.ReadFromJsonAsync<SmsLaneResponseDto>();

            if (smsLaneResult == null)
                throw new Exception("Unable to parse SMS Provider response.");

            if (smsLaneResult.ErrorCode != 0)
                throw new Exception($"SMS sending failed: {smsLaneResult.ErrorDescription}");

            // Save OTP to DB
            var otpEntity = new OtpMessage
            {
                MobileNumber = mobile,
                OtpCode = otpCode,
                Purpose = purpose,
                ExpiryAt = DateTime.UtcNow.AddMinutes(5)
            };

            await _otpRepository.CreateOtpAsync(otpEntity);
            return otpEntity;
        }

        public async Task<bool> VerifyOtpAsync(string mobile, string otp, string purpose)
        {
            OtpMessage? record = await _otpRepository.GetValidOtpAsync(mobile, otp, purpose);
            if (record != null)
            {
                await _otpRepository.MarkOtpAsUsedAsync(record);
                return true;
            }
            return false;
        }

        private static string GenerateOtp(int size)
        {
            if (size != 4 && size != 6)
                throw new ArgumentException("OTP size must be 4 or 6 digits long");

            var random = new Random();
            return string.Join("", Enumerable.Range(0, size).Select(_ => random.Next(0, 10)));
        }
    }
}
