using Microsoft.EntityFrameworkCore;
using Witcher3IngredientsMVC.Models;

namespace Witcher3IngredientsMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemLink> ItemLinks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemLink>()
         .HasOne(il => il.ResultItem)
         .WithMany() // no back navigation from Item
         .HasForeignKey(il => il.ResultItemId)
         .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
