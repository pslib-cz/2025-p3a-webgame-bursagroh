using game.Server.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;


namespace game.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemInstance> ItemInstances { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasData(new Player
                {
                    PlayerId = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"),
                    Name = "Seeded Player",
                    Money = 100,
                    BankBalance = 0, 
                    ScreenType = ScreenTypes.City, 
                    Capacity = 10, 
                    Seed = 252
                });
            });

            modelBuilder.Entity<InventoryItem>(entity =>
            {
                entity.HasData(new InventoryItem
                {
                    InventoryItemId = 52,
                    PlayerId = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"),
                    IsInBank = true,
                    ItemInstanceId = 85
                });
            });
        }

    }
}