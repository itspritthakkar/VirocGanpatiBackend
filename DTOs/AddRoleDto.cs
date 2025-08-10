namespace VirocGanpati.DTOs
{
    public class AddRoleDto
    {
        public string RoleName { get; set; }  // e.g., Manager, Manager-View, User

        public string RoleIdentifier { get; set; }  // Unique identifier for role, e.g., MGR, MGR-VIEW, USER

        public string RoleDescription { get; set; }  // Nullable field for additional description
    }
}
