using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Infrastructure.Identity.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user);
}
