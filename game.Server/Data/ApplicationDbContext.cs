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
        public DbSet<Building> Buildings { get; set; }

        public DbSet<ItemInstance> ItemInstances { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        public DbSet<Mine> Mines { get; set; }
        public DbSet<MineLayer> MineLayers { get; set; }
        public DbSet<MineBlock> MineBlocks { get; set; }
        public DbSet<Block> Blocks { get; set; }


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

            modelBuilder.Entity<Mine>(entity =>
            {
                entity.HasData(new Mine
                {
                    MineId = 1,
                    PlayerId = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"),
                });
            });

            modelBuilder.Entity<MineLayer>(entity =>
            {
                entity.HasData(new MineLayer
                {
                    MineLayerID = 1,
                    MineId = 1
                });
            });

            // You need to add seeding for the Items that your Blocks reference.
            modelBuilder.Entity<Item>().HasData(
                // Assuming you have an 'Item' model with properties like ItemId and Name
                new Item { ItemId = 101, Name = "Stone Resource" },
                new Item { ItemId = 202, Name = "Ore Resource" }
            );
            // --- Block Seeding comes after this ---
            modelBuilder.Entity<Block>().HasData(
                new Block { BlockId = 1, BlockType = BlockType.Stone, ItemId = 101, MinAmount = 1, MaxAmount = 3 },
                new Block { BlockId = 2, BlockType = BlockType.Ore, ItemId = 202, MinAmount = 1, MaxAmount = 1 }
            );
        }

    }
}