namespace Corporation.Dal.Data;

public class Plant1Context : DbContext
{
    public DbSet<ProductType> ProductTypes { get; set; }

    public Plant1Context(DbContextOptions<Plant1Context> options) : base(options)
    { }
}

