namespace VirocGanpati.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? MandalId { get; set; }
        public string MandalName { get; set; }
        public string MandalSlug { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleIdentifier { get; set; }
        public string Status { get; set; }
        public bool IsMobileVerified { get; set; }
    }
}
