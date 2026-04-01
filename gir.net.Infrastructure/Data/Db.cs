using gir.net.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace gir.net.Infrastructure.Data;

public class Db(DbContextOptions<Db> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<FilterWord> FilterWords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();
        
        modelBuilder.Entity<FilterWord>()
            .HasIndex(t => t.Phrase)
            .IsUnique();
    }
}
