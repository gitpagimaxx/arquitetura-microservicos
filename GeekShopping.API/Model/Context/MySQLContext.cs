using GeekShopping.API.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.API.Model.Context;

public class MySQLContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MySQLContext() {}

    public MySQLContext(
        DbContextOptions<MySQLContext> options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Product> Products { get; set; }
    
}
