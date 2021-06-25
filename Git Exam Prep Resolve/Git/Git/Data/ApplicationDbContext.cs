namespace Git.Data
{
    using Git.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Commit> Commits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Git;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Commit>(c => 
            {
                c.HasKey(x => x.Id);

                c.HasOne(x => x.Creator)
                .WithMany(x => x.Commits)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

                c.HasOne(x => x.Repository)
                .WithMany(x => x.Commits)
                .HasForeignKey(x => x.RepositoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}