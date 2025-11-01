using Duende.IdentityModel;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Services;

public class ProfileService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory) : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory = _userClaimsPrincipalFactory;

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string id = context.Subject.GetSubjectId();

        var user = _userManager.FindByIdAsync(id).Result;

        var userClaims = await _claimsFactory.CreateAsync(user!);

        var claims = userClaims.Claims.ToList();
        claims.Add(new Claim(JwtClaimTypes.FamilyName, user!.LastName!));
        claims.Add(new Claim(JwtClaimTypes.GivenName, user!.FirstName!));

        if (_userManager.SupportsUserRole)
        {
            var roles = await _userManager.GetRolesAsync(user!);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                if (_roleManager.SupportsRoleClaims)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        claims.AddRange(await _roleManager.GetClaimsAsync(role));
                    }
                }
            }
        }

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string id = context.Subject.GetSubjectId();

        var user = _userManager.FindByIdAsync(id).Result;

        context.IsActive = user != null;
    }
}