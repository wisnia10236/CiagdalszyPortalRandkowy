using Microsoft.EntityFrameworkCore;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)         // jesli tabela ma wiele do wielu jak like to musimy je pokazac
        {
            builder.Entity<Like>().HasKey(k => new { k.UserLikesId, k.UserIsLikedId });     // wskazujemy dla entity ze tabela like bedzie miala dwa klucze
            
             // wskazujemy ze uzytk moze byc lubiony przez wielu uzytkownikow
            builder.Entity<Like>().HasOne(u => u.UserIsLiked)
                                  .WithMany(u => u.UserLikes)
                                  .HasForeignKey(u => u.UserIsLikedId)
                                  .OnDelete(DeleteBehavior.Restrict);  // usuwanie kaskadowe

            builder.Entity<Like>().HasOne(u => u.UserLikes)     // wskazujemy ze uzytk moze lubic wielu uzytkownikow
                                  .WithMany(u => u.UserIsLiked)
                                  .HasForeignKey(u => u.UserLikesId)
                                  .OnDelete(DeleteBehavior.Restrict);  // usuwanie kaskadowe

        }
    }
}