using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public interface IOtpRepository
    {
        Task<OtpMessage> CreateOtpAsync(OtpMessage otp);
        Task<OtpMessage> UpdateOtpAsync(OtpMessage otp);
        Task<OtpMessage?> GetValidOtpAsync(string mobile, string otp, string purpose);
        Task MarkOtpAsUsedAsync(OtpMessage otp);
        Task<OtpMessage?> GetActiveOtpAsync(string mobile, string purpose);
    }
}
