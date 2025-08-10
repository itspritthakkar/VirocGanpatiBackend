using VirocGanpati.Models;

namespace VirocGanpati.Services
{
    public interface IOtpService
    {
        Task<OtpMessage> SendOtpAsync(string mobile, string purpose, int otpSize);
        Task<bool> VerifyOtpAsync(string mobile, string otp, string purpose);
    }
}
