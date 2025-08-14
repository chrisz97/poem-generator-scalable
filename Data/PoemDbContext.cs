using Microsoft.EntityFrameworkCore;
using PoemGenerator.Monolith.Data.Entities;

namespace PoemGenerator.Monolith.Data;

public class PoemDbContext : DbContext
{
    public PoemDbContext(DbContextOptions<PoemDbContext> options) : base(options)
    {
    }

    public DbSet<PoemEntity> Poems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PoemEntity>(entity =>
        {
            entity.ToTable("Poems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Content)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<AuthorEntity>(entity =>
        {
            entity.ToTable("Authors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            entity.HasIndex(e => e.Name)
                .IsUnique();
        });
    }
}