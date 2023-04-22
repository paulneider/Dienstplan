using Microsoft.EntityFrameworkCore;

namespace Dienstplan;
internal class ModelContext : DbContext
{
    public DbSet<Group> Groups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Dienstplan.db");
    }
}
