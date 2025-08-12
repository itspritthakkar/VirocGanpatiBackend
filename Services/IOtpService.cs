using VirocGanpati.Models;

namespace VirocGanpati.Services
{
    public interface IOtpService
    {
        Task<OtpMessage> SendOtpAsync(string mobile, string purpose, int otpSize, string firstName);
        Task<OtpMessage> ResendOtpAsync(string mobile, string purpose, int otpSize, string firstName);
        Task<bool> VerifyOtpAsync(string mobile, string otp, string purpose);
    }
}
