namespace CoreDatabase.Db;

using Microsoft.EntityFrameworkCore;
using Models;

public class LearningDbContext : DbContext
{
    public LearningDbContext(DbContextOptions options) : base(options)
    {
    }

    // The property name here, controls the table name that is generated
    public DbSet<Queue> Queues { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;

    public DbSet<SubModelA> SubModelAs { get; set; } = null!;
    public DbSet<SubModelB> SubModelBs { get; set; } = null!;
    // TODO: How can I query the base class of ICommonModel (or CommonModel whatever)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This can also be written in IEntityTypeConfiguration<T> classes
        // and registered here. I felt at the time that this was more work than
        // it was worth, and I am now exploring how this model feels.
        // 1. I like that I add the DbSet<T> above, and then tweak properties below as needed
        // 2. Previously I had to add the DbSet<T>, then add the mapping class, and then I would explicitly
        //    configure all of the properties. that was more work than it seemed worth

        modelBuilder.Entity<Queue>()
            .HasIndex(x => x.Name).IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.SearchVector)
            .HasColumnType("tsvector");
        modelBuilder.Entity<User>()
            .HasIndex(u => u.SearchVector)
            .HasMethod("GIN");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql()
            .UseSnakeCaseNamingConvention();
    }
}
