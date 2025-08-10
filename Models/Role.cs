using System.ComponentModel.DataAnnotations;

namespace VirocGanpati.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }  // e.g., Manager, Manager-View, User

        [Required]
        public string RoleIdentifier { get; set; }  // Unique identifier for role, e.g., MGR, MGR-VIEW, USER

        public string RoleDescription { get; set; }  // Nullable field for additional description
    }
}
