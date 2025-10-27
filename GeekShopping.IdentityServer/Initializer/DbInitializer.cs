using Duende.IdentityModel;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer;

public class DbInitializer(
    MySQLContext context,
    UserManager<ApplicationUser> user,
    RoleManager<IdentityRole> role) : IDbInitializer
{
    private readonly MySQLContext _context = context;
    private readonly UserManager<ApplicationUser> _user = user;
    private readonly RoleManager<IdentityRole> _role = role;

    public void Initialize()
    {
        if (_role.FindByNameAsync("Admin").Result != null) return;

        _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
        _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

        ApplicationUser admin = new()
        {
            UserName = "diego-admin",
            Email = "diego-admin@pagimax.cloud",
            EmailConfirmed = true,
            PhoneNumber = "+55 (11) 99130-6333",
            FirstName = "Diego",
            LastName = "Admin"
        };

        _user.CreateAsync(admin, "Diego@123").GetAwaiter().GetResult();
        _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

        var adminClaims = _user.AddClaimsAsync(admin,
        [
            new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
            new Claim(JwtClaimTypes.GivenName, admin.FirstName),
            new Claim(JwtClaimTypes.FamilyName, admin.LastName),
            new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
        ]).Result;

        ApplicationUser client = new()
        {
            UserName = "diego-client",
            Email = "diego-client@pagimax.cloud",
            EmailConfirmed = true,
            PhoneNumber = "+55 (11) 99130-6333",
            FirstName = "Diego",
            LastName = "Client"
        };

        _user.CreateAsync(client, "Diego@123").GetAwaiter().GetResult();
        _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

        var clientClaims = _user.AddClaimsAsync(client,
        [
            new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
            new Claim(JwtClaimTypes.GivenName, client.FirstName),
            new Claim(JwtClaimTypes.FamilyName, client.LastName),
            new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
        ]).Result;
    }
}
