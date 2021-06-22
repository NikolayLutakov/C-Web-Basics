using BattleCards.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BattleCards.Data
{
    public class ApplicationDbContext : DbContext
    { 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<UserCard> UserCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserCard>(userCard => 
            {
                userCard.HasKey(x => new { x.UserId, x.CardId });

                userCard.HasOne(x => x.User)
                .WithMany(x => x.UserCards)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                userCard.HasOne(x => x.Card)
                .WithMany(x => x.UserCards)
                .HasForeignKey(x => x.CardId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
