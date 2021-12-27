namespace Corporation.Dal.Data;

public class CorporationContext : IdentityDbContext<User, Role, string>
{
    public DbSet<Corp> Corps { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Workstation> Workstations { get; set; }


    public DbSet<ProductType> ProductTypes { get; set; }
    public CorporationContext(DbContextOptions<CorporationContext> options) : base(options)
    { }
}

