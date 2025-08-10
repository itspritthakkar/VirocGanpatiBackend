namespace VirocGanpati.DTOs.Payments
{
    public class RazorpayVerificationRequestDto
    {
        public string RazorpayOrderId { get; set; }

        public string RazorpayPaymentId { get; set; }

        public string RazorpaySignature { get; set; }

        public string Password { get; set; }
    }
}
