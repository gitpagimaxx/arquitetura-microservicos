using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.IdentityServer.Model.Context;

public class MySQLContext : IdentityDbContext<ApplicationUser>
{
    private readonly IConfiguration? _configuration;

    public MySQLContext(
        DbContextOptions<MySQLContext> options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
}
