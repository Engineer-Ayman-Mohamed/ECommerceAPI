using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public AppUserAddress? Address { get; set; }
}
