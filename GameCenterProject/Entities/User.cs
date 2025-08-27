using System;

namespace GameCenterProject.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "user"; // "user" or "admin"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
