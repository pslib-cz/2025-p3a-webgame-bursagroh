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

        public DbSet<Floor> Floors { get; set; }

        public DbSet<FloorItem> FloorItems { get; set; }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredience> Ingrediences { get; set; }

        public DbSet<RecipeTime> RecipeTimes { get; set; }


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

            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasData(new Building
                {
                    PlayerId = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0"),
                    BuildingId = 69
                });
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.HasData(new Floor
                {
                    FloorId = 6,
                    BuildingId = 69
                });
            });

            modelBuilder.Entity<FloorItem>(entity =>
            {
                entity.HasData(new FloorItem
                {
                    FloorId = 6,
                    FloorItemId = 85
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
                    MineId = 1
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


            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasData(
                    new Recipe { RecipeId = 1, Name = "Hamburger"}
                );
            });

            modelBuilder.Entity<Ingredience>(entity =>
            {
                entity.HasData(
                    new Ingredience { RecipeId = 1, IngredienceId = 1, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 1, IngredienceId = 2, Order = 2, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 1, IngredienceId = 3, Order = 3, IngredienceType = IngredienceTypes.BunDown }
                );
            });

            modelBuilder.Entity<RecipeTime>(entity =>
            {
                entity.HasData(
                    new RecipeTime { RecipeTimeId = 1, RecipeId = 1, StartTime = new DateTime(2025, 12, 13, 10, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 13, 11, 0, 0, DateTimeKind.Utc), PlayerId = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0") }
                );
            });

            modelBuilder.Entity<Item>().HasData(
                new Item { ItemId = 1, Name = "Rock" },
                new Item { ItemId = 2, Name = "Iron Ore" },
                new Item { ItemId = 3, Name = "Copper Ore" },
                new Item { ItemId = 4, Name = "Silver Ore" },
                new Item { ItemId = 5, Name = "Gold Ore" },
                new Item { ItemId = 6, Name = "Unobtainium Ore" }
            );
            modelBuilder.Entity<Block>().HasData(
                new Block { BlockId = 1, BlockType = BlockType.Rock, ItemId = 1, MinAmount = 1, MaxAmount = 3},
                new Block { BlockId = 2, BlockType = BlockType.Iron_Ore, ItemId = 2, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 3, BlockType = BlockType.Copper_Ore, ItemId = 3, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 4, BlockType = BlockType.Silver_Ore, ItemId = 4, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 5, BlockType = BlockType.Gold_Ore, ItemId = 5, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 6, BlockType = BlockType.Unobtanium_Ore, ItemId = 6, MinAmount = 1, MaxAmount = 1 }
            );
        }

    }
}