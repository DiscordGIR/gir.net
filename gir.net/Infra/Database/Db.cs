using Microsoft.EntityFrameworkCore;

namespace gir.net.Infra.Database;

public class Db(DbContextOptions<Db> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<ButtonLink> ButtonLinks { get; set; } = null!;
}
