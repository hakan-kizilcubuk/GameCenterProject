using Microsoft.AspNetCore.Identity;

namespace GameCenterProject.Infrastructure.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }
    }
}
