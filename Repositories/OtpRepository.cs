using Microsoft.EntityFrameworkCore;
using VirocGanpati.Data;
using VirocGanpati.Models;

namespace VirocGanpati.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public OtpRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OtpMessage> CreateOtpAsync(OtpMessage otp)
        {
            _context.OtpMessages.Add(otp);
            await _context.SaveChangesAsync();
            return otp;
        }

        public async Task<OtpMessage> UpdateOtpAsync(OtpMessage otp)
        {
            _context.OtpMessages.Update(otp);
            await _context.SaveChangesAsync();
            return otp;
        }

        public async Task<OtpMessage?> GetValidOtpAsync(string mobile, string otp, string purpose)
        {
            return await _context.OtpMessages
                .Where(o => o.MobileNumber == mobile &&
                            o.OtpCode == otp &&
                            o.Purpose == purpose &&
                            !o.IsUsed &&
                            o.ExpiryAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task MarkOtpAsUsedAsync(OtpMessage otp)
        {
            otp.IsUsed = true;
            await _context.SaveChangesAsync();
        }

        public async Task<OtpMessage?> GetActiveOtpAsync(string mobile, string purpose)
        {
            return await _context.OtpMessages
                .Where(o => o.MobileNumber == mobile && o.Purpose == purpose && o.ExpiryAt > DateTime.UtcNow)
                .OrderByDescending(o => o.CreatedAt) // If you store creation time
                .FirstOrDefaultAsync();
        }
    }
}
