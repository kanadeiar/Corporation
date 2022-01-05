using Corporation.Domain.Company1;

namespace Corporation.Dal.Data;

public class CorporationContext : IdentityDbContext<User, Role, string>
{
    public DbSet<Corp> Corps { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Workstation> Workstations { get; set; }

    public DbSet<Com1Shift> Com1Shifts { get; set; }
    public DbSet<Com1LooseRaw> Com1LooseRaws { get; set; }
    public DbSet<Com1Warehouse1ShiftData> Com1Warehouse1ShiftDatas { get; set; }
    public DbSet<Com1ProductType> Com1ProductTypes { get; set; }
    public DbSet<Com1Press1ShiftData> Com1Press1ShiftDatas { get; set; }
    public DbSet<Com1Autoclave> Com1Autoclaves { get; set; }
    public DbSet<Com1Autoclaves1ShiftData> Com1Autoclaves1ShiftDatas { get; set; }
    public DbSet<Com1Pack> Com1Packs { get; set; }
    public DbSet<Com1Packing1ShiftData> Com1Packing1ShiftDatas { get; set; }
    public DbSet<Com1Warehouse2ShiftData> Com1Warehouse2ShiftDatas { get; set; }

    public CorporationContext(DbContextOptions<CorporationContext> options) : base(options)
    { }
}

