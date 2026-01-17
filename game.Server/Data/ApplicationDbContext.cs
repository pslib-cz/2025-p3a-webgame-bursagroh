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

        public DbSet<InventoryItem> ItemMineBlocks { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<FloorItem> FloorItems { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredience> Ingrediences { get; set; }
        public DbSet<RecipeTime> RecipeTimes { get; set; }
        public DbSet<Blueprint> Blueprints { get; set; }
        public DbSet<Crafting> Craftings { get; set; }

        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Chest> Chests { get; set; }

        public DbSet<BlueprintPlayer> BlueprintPlayers { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>()
                .Navigation(p => p.Floor)
                .AutoInclude();

            modelBuilder.Entity<Floor>()
                .Navigation(f => f.FloorItems)
                .AutoInclude();

            modelBuilder.Entity<FloorItem>()
                .Navigation(fi => fi.Chest)
                .AutoInclude();

            modelBuilder.Entity<FloorItem>()
                .Navigation(fi => fi.Enemy)
                .AutoInclude();

            modelBuilder.Entity<InventoryItem>()
                .Navigation(i => i.ItemInstance)
                .AutoInclude();

            modelBuilder.Entity<ItemInstance>()
                .Navigation(ii => ii.Item)
                .AutoInclude();


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

            modelBuilder.Entity<FloorItem>()
                .HasOne(fi => fi.ItemInstance)
                .WithMany()
                .HasForeignKey(fi => fi.ItemInstanceId)
                .OnDelete(DeleteBehavior.Restrict);






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
                    new Recipe { RecipeId = 1, Name = "Classic Hamburger" },
                    new Recipe { RecipeId = 2, Name = "Cheeseburger" },
                    new Recipe { RecipeId = 3, Name = "Bacon Burger" },
                    new Recipe { RecipeId = 4, Name = "Veggie Deluxe" },
                    new Recipe { RecipeId = 5, Name = "BLT Sandwich" },
                    new Recipe { RecipeId = 6, Name = "Chicken Sandwich" },
                    new Recipe { RecipeId = 7, Name = "The Works Burger" },
                    new Recipe { RecipeId = 8, Name = "Simple Salad Burger" },
                    new Recipe { RecipeId = 9, Name = "Triple Meat Stack" },
                    new Recipe { RecipeId = 10, Name = "Saucy Bacon Tomato Burger" },
                    new Recipe { RecipeId = 11, Name = "Bacon Cheeseburger Deluxe" }
                );
            });

            modelBuilder.Entity<Ingredience>(entity =>
            {
                entity.HasData(
                    // --- Recipe 1: Classic Hamburger ---
                    new Ingredience { RecipeId = 1, IngredienceId = 1, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 1, IngredienceId = 2, Order = 2, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 1, IngredienceId = 3, Order = 3, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 2: Cheeseburger (FIXED: Sauce -> Cheese) ---
                    new Ingredience { RecipeId = 2, IngredienceId = 4, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 2, IngredienceId = 5, Order = 2, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 2, IngredienceId = 6, Order = 3, IngredienceType = IngredienceTypes.Cheese }, 
                    new Ingredience { RecipeId = 2, IngredienceId = 7, Order = 4, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 3: Bacon Burger ---
                    new Ingredience { RecipeId = 3, IngredienceId = 8, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 3, IngredienceId = 9, Order = 2, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 3, IngredienceId = 10, Order = 3, IngredienceType = IngredienceTypes.Bacon },
                    new Ingredience { RecipeId = 3, IngredienceId = 11, Order = 4, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 4: Veggie Deluxe ---
                    new Ingredience { RecipeId = 4, IngredienceId = 12, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 4, IngredienceId = 13, Order = 2, IngredienceType = IngredienceTypes.Salad },
                    new Ingredience { RecipeId = 4, IngredienceId = 14, Order = 3, IngredienceType = IngredienceTypes.Tomato },
                    new Ingredience { RecipeId = 4, IngredienceId = 15, Order = 4, IngredienceType = IngredienceTypes.Sauce },
                    new Ingredience { RecipeId = 4, IngredienceId = 16, Order = 5, IngredienceType = IngredienceTypes.Salad },
                    new Ingredience { RecipeId = 4, IngredienceId = 17, Order = 6, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 5: BLT Sandwich ---
                    new Ingredience { RecipeId = 5, IngredienceId = 18, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 5, IngredienceId = 19, Order = 2, IngredienceType = IngredienceTypes.Bacon },
                    new Ingredience { RecipeId = 5, IngredienceId = 20, Order = 3, IngredienceType = IngredienceTypes.Salad },
                    new Ingredience { RecipeId = 5, IngredienceId = 21, Order = 4, IngredienceType = IngredienceTypes.Tomato },
                    new Ingredience { RecipeId = 5, IngredienceId = 22, Order = 5, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 6: Chicken Sandwich ---
                    new Ingredience { RecipeId = 6, IngredienceId = 23, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 6, IngredienceId = 24, Order = 2, IngredienceType = IngredienceTypes.Sauce },
                    new Ingredience { RecipeId = 6, IngredienceId = 25, Order = 3, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 6, IngredienceId = 26, Order = 4, IngredienceType = IngredienceTypes.Salad },
                    new Ingredience { RecipeId = 6, IngredienceId = 27, Order = 5, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 7: The Works Burger ---
                    new Ingredience { RecipeId = 7, IngredienceId = 28, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 7, IngredienceId = 29, Order = 2, IngredienceType = IngredienceTypes.Sauce },
                    new Ingredience { RecipeId = 7, IngredienceId = 30, Order = 3, IngredienceType = IngredienceTypes.Tomato },
                    new Ingredience { RecipeId = 7, IngredienceId = 31, Order = 4, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 7, IngredienceId = 32, Order = 5, IngredienceType = IngredienceTypes.Bacon },
                    new Ingredience { RecipeId = 7, IngredienceId = 33, Order = 6, IngredienceType = IngredienceTypes.Salad },
                    new Ingredience { RecipeId = 7, IngredienceId = 34, Order = 7, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 8: Simple Salad Burger ---
                    new Ingredience { RecipeId = 8, IngredienceId = 35, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 8, IngredienceId = 36, Order = 2, IngredienceType = IngredienceTypes.Salad },
                    new Ingredience { RecipeId = 8, IngredienceId = 37, Order = 3, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 8, IngredienceId = 38, Order = 4, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 9: Triple Meat Stack ---
                    new Ingredience { RecipeId = 9, IngredienceId = 39, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 9, IngredienceId = 40, Order = 2, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 9, IngredienceId = 41, Order = 3, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 9, IngredienceId = 42, Order = 4, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 9, IngredienceId = 43, Order = 5, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 10: Saucy Bacon Tomato Burger ---
                    new Ingredience { RecipeId = 10, IngredienceId = 44, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 10, IngredienceId = 45, Order = 2, IngredienceType = IngredienceTypes.Sauce },
                    new Ingredience { RecipeId = 10, IngredienceId = 46, Order = 3, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 10, IngredienceId = 47, Order = 4, IngredienceType = IngredienceTypes.Bacon },
                    new Ingredience { RecipeId = 10, IngredienceId = 48, Order = 5, IngredienceType = IngredienceTypes.Tomato },
                    new Ingredience { RecipeId = 10, IngredienceId = 49, Order = 6, IngredienceType = IngredienceTypes.BunDown },

                    // --- Recipe 11: Bacon Cheeseburger Deluxe ---
                    new Ingredience { RecipeId = 11, IngredienceId = 50, Order = 1, IngredienceType = IngredienceTypes.BunUp },
                    new Ingredience { RecipeId = 11, IngredienceId = 51, Order = 2, IngredienceType = IngredienceTypes.Salad },
                    new Ingredience { RecipeId = 11, IngredienceId = 52, Order = 3, IngredienceType = IngredienceTypes.Tomato },
                    new Ingredience { RecipeId = 11, IngredienceId = 53, Order = 4, IngredienceType = IngredienceTypes.Cheese }, 
                    new Ingredience { RecipeId = 11, IngredienceId = 54, Order = 5, IngredienceType = IngredienceTypes.Meat },
                    new Ingredience { RecipeId = 11, IngredienceId = 55, Order = 6, IngredienceType = IngredienceTypes.Bacon },
                    new Ingredience { RecipeId = 11, IngredienceId = 56, Order = 7, IngredienceType = IngredienceTypes.BunDown }
                );
            });

            modelBuilder.Entity<RecipeTime>(entity =>
            {
                entity.HasData(
                    new RecipeTime { RecipeTimeId = 1, RecipeId = 1, StartTime = new DateTime(2025, 12, 13, 10, 0, 0, DateTimeKind.Utc), EndTime = new DateTime(2025, 12, 13, 11, 0, 0, DateTimeKind.Utc), PlayerId = new Guid("4b1e8a93-7d92-4f7f-80c1-525c345b85e0") }
                );
            });

            modelBuilder.Entity<Item>().HasData(
                new Item { ItemId = 1, Name = "Wooden Frame",         Description = "Wooden Frame",        ItemType = ItemTypes.Block,   ChangeOfGenerating = 20 },
                new Item { ItemId = 2, Name = "Rock",                 Description = "Rock",                ItemType = ItemTypes.Block,   ChangeOfGenerating = 80 },
                new Item { ItemId = 3, Name = "Copper Ore",           Description = "Copper Ore",          ItemType = ItemTypes.Block,   ChangeOfGenerating = 15 },
                new Item { ItemId = 4, Name = "Iron Ore",             Description = "Iron Ore",            ItemType = ItemTypes.Block,   ChangeOfGenerating = 15 },
                new Item { ItemId = 5, Name = "Silver Ore",           Description = "Silver Ore",          ItemType = ItemTypes.Block,   ChangeOfGenerating = 10 },
                new Item { ItemId = 6, Name = "Gold Ore",             Description = "Gold Ore",            ItemType = ItemTypes.Block,   ChangeOfGenerating = 10 },
                new Item { ItemId = 7, Name = "Unobtainium Ore",      Description = "Unobtainium Ore",     ItemType = ItemTypes.Block,   ChangeOfGenerating = 2 },
                
                new Item { ItemId = 10, Name = "Wooden Sword",        Description = "Wooden Sword",        ItemType = ItemTypes.Sword,   Weight = 1, Damage = 10, MaxDurability = 20 },
                new Item { ItemId = 11, Name = "Rock Sword",          Description = "Rock Sword",          ItemType = ItemTypes.Sword,   Weight = 1, Damage = 20, MaxDurability = 40 },
                new Item { ItemId = 12, Name = "Copper Sword",        Description = "Copper Sword",        ItemType = ItemTypes.Sword,   Weight = 1, Damage = 30, MaxDurability = 60 },
                new Item { ItemId = 13, Name = "Iron Sword",          Description = "Iron Sword",          ItemType = ItemTypes.Sword,   Weight = 1, Damage = 40, MaxDurability = 80 },
                new Item { ItemId = 14, Name = "Silver Sword",        Description = "Silver Sword",        ItemType = ItemTypes.Sword,   Weight = 1, Damage = 50, MaxDurability = 100 },
                new Item { ItemId = 15, Name = "Gold Sword",          Description = "Gold Sword",          ItemType = ItemTypes.Sword,   Weight = 1, Damage = 60, MaxDurability = 120 },
                new Item { ItemId = 16, Name = "Unobtainium Sword",   Description = "Unobtainium Sword",   ItemType = ItemTypes.Sword,   Weight = 1, Damage = 70, MaxDurability = 240 },

                new Item { ItemId = 20, Name = "Wooden Axe",          Description = "Wooden Axe",          ItemType = ItemTypes.Axe,     Weight = 1, Damage = 1, MaxDurability = 20 },
                new Item { ItemId = 21, Name = "Rock Axe",            Description = "Rock Axe",            ItemType = ItemTypes.Axe,     Weight = 1, Damage = 2, MaxDurability = 40 },
                new Item { ItemId = 22, Name = "Copper Axe",          Description = "Copper Axe",          ItemType = ItemTypes.Axe,     Weight = 1, Damage = 3, MaxDurability = 60 },
                new Item { ItemId = 23, Name = "Iron Axe",            Description = "Iron Axe",            ItemType = ItemTypes.Axe,     Weight = 1, Damage = 4, MaxDurability = 80 },
                new Item { ItemId = 24, Name = "Silver Axe",          Description = "Silver Axe",          ItemType = ItemTypes.Axe,     Weight = 1, Damage = 5, MaxDurability = 100 },
                new Item { ItemId = 25, Name = "Gold Axe",            Description = "Gold Axe",            ItemType = ItemTypes.Axe,     Weight = 1, Damage = 6, MaxDurability = 120 },
                new Item { ItemId = 26, Name = "Unobtainium Axe",     Description = "Unobtainium Axe",     ItemType = ItemTypes.Axe,     Weight = 1, Damage = 7, MaxDurability = 240 },

                new Item { ItemId = 30, Name = "Wooden Pickaxe",      Description = "Wooden Pickaxe",      ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 1, MaxDurability = 20 },
                new Item { ItemId = 31, Name = "Rock Pickaxe",        Description = "Rock Pickaxe",        ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 2, MaxDurability = 40 },
                new Item { ItemId = 32, Name = "Copper Pickaxe",      Description = "Copper Pickaxe",      ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 3, MaxDurability = 60 },
                new Item { ItemId = 33, Name = "Iron Pickaxe",        Description = "Iron Pickaxe",        ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 4, MaxDurability = 80 },
                new Item { ItemId = 34, Name = "Silver Pickaxe",      Description = "Silver Pickaxe",      ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 5, MaxDurability = 100 },
                new Item { ItemId = 35, Name = "Gold Pickaxe",        Description = "Gold Pickaxe",        ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 6, MaxDurability = 120 },
                new Item { ItemId = 36, Name = "Unobtainium Pickaxe", Description = "Unobtainium Pickaxe", ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 7, MaxDurability = 240 },

                new Item { ItemId = 39, Name = "Rented Pickaxe", Description = "Rented Pickaxe", ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 1, MaxDurability = 15 }
            );
            modelBuilder.Entity<Block>().HasData(
                new Block { BlockId = 1, BlockType = BlockType.Wooden_Frame,   ItemId = 1, MinAmount = 1, MaxAmount = 1},
                new Block { BlockId = 2, BlockType = BlockType.Rock,           ItemId = 2, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 3, BlockType = BlockType.Copper_Ore,     ItemId = 3, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 4, BlockType = BlockType.Iron_Ore,       ItemId = 4, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 5, BlockType = BlockType.Silver_Ore,     ItemId = 5, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 6, BlockType = BlockType.Gold_Ore,       ItemId = 6, MinAmount = 1, MaxAmount = 1 },
                new Block { BlockId = 7, BlockType = BlockType.Unobtanium_Ore, ItemId = 7, MinAmount = 1, MaxAmount = 1 }
            );

            modelBuilder.Entity<Blueprint>()
                .Navigation(b => b.Craftings)
                .AutoInclude();

            modelBuilder.Entity<Blueprint>(entity =>
            {
                entity.HasData(
                    new Blueprint { BlueprintId = 1, ItemId = 10, Price = 5 },
                    new Blueprint { BlueprintId = 2, ItemId = 30, Price = 5 }
                );
            });

            modelBuilder.Entity<Crafting>(entity =>
            {
                entity.HasData(
                    new Crafting { CraftingId = 1, BlueprintId = 1, ItemId = 1, Amount = 2 },
                    new Crafting { CraftingId = 2, BlueprintId = 2, ItemId = 1, Amount = 3 }
                );
            });
            
            modelBuilder.Entity<BlueprintPlayer>()
                .HasKey(bp => new { bp.PlayerId, bp.BlueprintId });

            modelBuilder.Entity<BlueprintPlayer>()
                .HasOne(bp => bp.Player)
                .WithMany()
                .HasForeignKey(bp => bp.PlayerId);

            modelBuilder.Entity<BlueprintPlayer>()
                .HasOne(bp => bp.Blueprint)
                .WithMany()
                .HasForeignKey(bp => bp.BlueprintId);

            modelBuilder.Entity<ItemMineBlock>()
                .HasKey(imb => new { imb.ItemInstanceId, imb.MineBlockId });

            
            modelBuilder.Entity<ItemMineBlock>()
                .HasOne(imb => imb.ItemInstance)
                .WithMany()
                .HasForeignKey(imb => imb.ItemInstanceId);

            modelBuilder.Entity<ItemMineBlock>()
                .HasOne(imb => imb.MineBlock)
                .WithMany()
                .HasForeignKey(imb => imb.MineBlockId);

            modelBuilder.Entity<Blueprint>()
                .Navigation(b => b.Item)
                .AutoInclude();

            modelBuilder.Entity<Crafting>()
                .Navigation(c => c.Item)
                .AutoInclude();

            modelBuilder.Entity<Floor>()
                .Navigation(f => f.FloorItems)
                .AutoInclude();

            modelBuilder.Entity<FloorItem>()
                .Navigation(fi => fi.Enemy)
                .AutoInclude();

            modelBuilder.Entity<Enemy>()
                .Navigation(e => e.ItemInstance)
                .AutoInclude();

            modelBuilder.Entity<ItemInstance>()
                .Navigation(ii => ii.Item)
                .AutoInclude();

        }   
    }
}