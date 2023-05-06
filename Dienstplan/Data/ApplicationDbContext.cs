using Microsoft.EntityFrameworkCore;

namespace Dienstplan;

internal class ApplicationDbContext : DbContext
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Roster> Rosters { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<Shift> Shifts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=Dienstplan.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Roster>()
            .HasMany(e => e.Employees)
            .WithMany();
    }
}
