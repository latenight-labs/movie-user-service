using Microsoft.EntityFrameworkCore;
using Movie.User.Service.Domain.Entities;
using Movie.User.Service.Infra.Data.Configurations;

namespace Movie.User.Service.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Movie.User.Service.Domain.Entities.User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}