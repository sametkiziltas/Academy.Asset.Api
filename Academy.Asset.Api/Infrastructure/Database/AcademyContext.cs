using Microsoft.EntityFrameworkCore;

namespace Academy.Asset.Api.Infrastructure.Database;

using Domain;

public class AcademyContext : DbContext
{
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public AcademyContext(DbContextOptions<AcademyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}