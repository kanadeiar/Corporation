namespace Corporation.Dal.Data;

public class CorporationContext : IdentityDbContext<User, IdentityRole, string>
{
    public DbSet<ProductType> ProductTypes { get; set; }
    public CorporationContext(DbContextOptions<CorporationContext> options) : base(options)
    { }
}

