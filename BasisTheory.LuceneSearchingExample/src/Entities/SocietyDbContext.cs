using Microsoft.EntityFrameworkCore;

namespace BasisTheory.LuceneSearchingExample.entities;

public class SocietyDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    public SocietyDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(Person.Configure);
    }
}