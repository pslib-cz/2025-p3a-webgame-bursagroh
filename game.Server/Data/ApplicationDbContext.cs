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

        public DbSet<Save> Saves { get; set; }

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
                new Item { ItemId = 1, Name = "Wooden Frame",         Description = "Wooden Frame",        ItemType = ItemTypes.Block,   ChangeOfGenerating = 6 },
                new Item { ItemId = 2, Name = "Rock",                 Description = "Rock",                ItemType = ItemTypes.Block,   ChangeOfGenerating = 95 },
                new Item { ItemId = 3, Name = "Copper Ore",           Description = "Copper Ore",          ItemType = ItemTypes.Block,   ChangeOfGenerating = 7 },
                new Item { ItemId = 4, Name = "Iron Ore",             Description = "Iron Ore",            ItemType = ItemTypes.Block,   ChangeOfGenerating = 6 },
                new Item { ItemId = 5, Name = "Silver Ore",           Description = "Silver Ore",          ItemType = ItemTypes.Block,   ChangeOfGenerating = 7 },
                new Item { ItemId = 6, Name = "Gold Ore",             Description = "Gold Ore",            ItemType = ItemTypes.Block,   ChangeOfGenerating = 6 },
                new Item { ItemId = 7, Name = "Unobtainium Ore",      Description = "Unobtainium Ore",     ItemType = ItemTypes.Block,   ChangeOfGenerating = 1 },
                
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

                new Item { ItemId = 39, Name = "Rented Pickaxe", Description = "Rented Pickaxe", ItemType = ItemTypes.Pickaxe, Weight = 1, Damage = 1, MaxDurability = 15 },

                new Item { ItemId = 40, Name = "Healing Potion", Description = "This thing heals", ItemType = ItemTypes.Potion, Weight = 1, Damage = 1, MaxDurability = 1 },
                new Item { ItemId = 41, Name = "God Potion", Description = "+5 hp", ItemType = ItemTypes.Potion, Weight = 1, Damage = 1, MaxDurability = 1 },
                new Item { ItemId = 42, Name = "Muscle Potion", Description = "+5 inventory space", ItemType = ItemTypes.Potion, Weight = 1, Damage = 1, MaxDurability = 1 }

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

            modelBuilder.Entity<Blueprint>().HasData(
                // Swords
                new Blueprint { BlueprintId = 1, ItemId = 10, Price = 5 },   // Wooden Sword
                new Blueprint { BlueprintId = 3, ItemId = 11, Price = 10 },  // Rock Sword
                new Blueprint { BlueprintId = 4, ItemId = 12, Price = 20 },  // Copper Sword
                new Blueprint { BlueprintId = 5, ItemId = 13, Price = 40 },  // Iron Sword
                new Blueprint { BlueprintId = 6, ItemId = 14, Price = 80 },  // Silver Sword
                new Blueprint { BlueprintId = 7, ItemId = 15, Price = 150 }, // Gold Sword
                new Blueprint { BlueprintId = 8, ItemId = 16, Price = 500 }, // Unobtainium Sword

                // Axes
                new Blueprint { BlueprintId = 9, ItemId = 20, Price = 5 },   // Wooden Axe
                new Blueprint { BlueprintId = 10, ItemId = 21, Price = 10 }, // Rock Axe
                new Blueprint { BlueprintId = 11, ItemId = 22, Price = 20 }, // Copper Axe
                new Blueprint { BlueprintId = 12, ItemId = 23, Price = 40 }, // Iron Axe
                new Blueprint { BlueprintId = 13, ItemId = 24, Price = 80 }, // Silver Axe
                new Blueprint { BlueprintId = 14, ItemId = 25, Price = 150 },// Gold Axe
                new Blueprint { BlueprintId = 15, ItemId = 26, Price = 500 },// Unobtainium Axe

                // Pickaxes
                new Blueprint { BlueprintId = 2, ItemId = 30, Price = 5 },   // Wooden Pickaxe
                new Blueprint { BlueprintId = 16, ItemId = 31, Price = 10 }, // Rock Pickaxe
                new Blueprint { BlueprintId = 17, ItemId = 32, Price = 20 }, // Copper Pickaxe
                new Blueprint { BlueprintId = 18, ItemId = 33, Price = 40 }, // Iron Pickaxe
                new Blueprint { BlueprintId = 19, ItemId = 34, Price = 80 }, // Silver Pickaxe
                new Blueprint { BlueprintId = 20, ItemId = 35, Price = 150 },// Gold Pickaxe
                new Blueprint { BlueprintId = 21, ItemId = 36, Price = 500 }, // Unobtainium Pickaxe

                new Blueprint { BlueprintId = 22, ItemId = 40, Price = 500 },
                new Blueprint { BlueprintId = 23, ItemId = 41, Price = 500 },
                new Blueprint { BlueprintId = 24, ItemId = 42, Price = 500 } 
            );

            modelBuilder.Entity<Crafting>().HasData(
                // --- WOODEN TIER (Basics) ---
                new Crafting { CraftingId = 1, BlueprintId = 1, ItemId = 1, Amount = 2 },  // Wooden Sword: 2 Wood
                new Crafting { CraftingId = 2, BlueprintId = 2, ItemId = 1, Amount = 3 },  // Wooden Pickaxe: 3 Wood
                new Crafting { CraftingId = 40, BlueprintId = 9, ItemId = 1, Amount = 3 }, // Wooden Axe: 3 Wood

                // --- SWORDS (Wood + Material) ---
                // Rock Sword: 2 Wood + 3 Rock = 5
                new Crafting { CraftingId = 3, BlueprintId = 3, ItemId = 1, Amount = 2 },
                new Crafting { CraftingId = 4, BlueprintId = 3, ItemId = 2, Amount = 3 },
                // Copper Sword: 2 Wood + 5 Copper = 7
                new Crafting { CraftingId = 5, BlueprintId = 4, ItemId = 1, Amount = 2 },
                new Crafting { CraftingId = 6, BlueprintId = 4, ItemId = 3, Amount = 5 },
                // Iron Sword: 3 Wood + 6 Iron = 9
                new Crafting { CraftingId = 7, BlueprintId = 5, ItemId = 1, Amount = 3 },
                new Crafting { CraftingId = 8, BlueprintId = 5, ItemId = 4, Amount = 6 },
                // Silver Sword: 3 Wood + 8 Silver = 11
                new Crafting { CraftingId = 9, BlueprintId = 6, ItemId = 1, Amount = 3 },
                new Crafting { CraftingId = 10, BlueprintId = 6, ItemId = 5, Amount = 8 },
                // Gold Sword: 4 Wood + 9 Gold = 13
                new Crafting { CraftingId = 11, BlueprintId = 7, ItemId = 1, Amount = 4 },
                new Crafting { CraftingId = 12, BlueprintId = 7, ItemId = 6, Amount = 9 },
                // Unobtainium Sword: 5 Wood + 10 Unobtainium = 15 (MAX)
                new Crafting { CraftingId = 13, BlueprintId = 8, ItemId = 1, Amount = 5 },
                new Crafting { CraftingId = 14, BlueprintId = 8, ItemId = 7, Amount = 10 },

                // --- AXES ---
                // Rock Axe: 3 Wood + 3 Rock = 6
                new Crafting { CraftingId = 16, BlueprintId = 10, ItemId = 1, Amount = 3 },
                new Crafting { CraftingId = 17, BlueprintId = 10, ItemId = 2, Amount = 3 },
                // Copper Axe: 3 Wood + 5 Copper = 8
                new Crafting { CraftingId = 41, BlueprintId = 11, ItemId = 1, Amount = 3 },
                new Crafting { CraftingId = 42, BlueprintId = 11, ItemId = 3, Amount = 5 },
                // Iron Axe: 4 Wood + 7 Iron = 11
                new Crafting { CraftingId = 18, BlueprintId = 12, ItemId = 1, Amount = 4 },
                new Crafting { CraftingId = 19, BlueprintId = 12, ItemId = 4, Amount = 7 },
                // Silver Axe: 4 Wood + 9 Silver = 13
                new Crafting { CraftingId = 43, BlueprintId = 13, ItemId = 1, Amount = 4 },
                new Crafting { CraftingId = 44, BlueprintId = 13, ItemId = 5, Amount = 9 },
                // Gold Axe: 5 Wood + 10 Gold = 15 (MAX)
                new Crafting { CraftingId = 45, BlueprintId = 14, ItemId = 1, Amount = 5 },
                new Crafting { CraftingId = 46, BlueprintId = 14, ItemId = 6, Amount = 10 },
                // Unobtainium Axe: 5 Wood + 10 Unobtainium = 15 (MAX)
                new Crafting { CraftingId = 20, BlueprintId = 15, ItemId = 1, Amount = 5 },
                new Crafting { CraftingId = 21, BlueprintId = 15, ItemId = 7, Amount = 10 },

                // --- PICKAXES ---
                // Rock Pickaxe: 3 Wood + 4 Rock = 7
                new Crafting { CraftingId = 22, BlueprintId = 16, ItemId = 1, Amount = 3 },
                new Crafting { CraftingId = 23, BlueprintId = 16, ItemId = 2, Amount = 4 },
                // Copper Pickaxe: 3 Wood + 6 Copper = 9
                new Crafting { CraftingId = 47, BlueprintId = 17, ItemId = 1, Amount = 3 },
                new Crafting { CraftingId = 48, BlueprintId = 17, ItemId = 3, Amount = 6 },
                // Iron Pickaxe: 4 Wood + 8 Iron = 12
                new Crafting { CraftingId = 49, BlueprintId = 18, ItemId = 1, Amount = 4 },
                new Crafting { CraftingId = 50, BlueprintId = 18, ItemId = 4, Amount = 8 },
                // Silver Pickaxe: 4 Wood + 8 Silver = 12
                new Crafting { CraftingId = 24, BlueprintId = 19, ItemId = 1, Amount = 4 },
                new Crafting { CraftingId = 25, BlueprintId = 19, ItemId = 5, Amount = 8 },
                // Gold Pickaxe: 5 Wood + 10 Gold = 15 (MAX)
                new Crafting { CraftingId = 51, BlueprintId = 20, ItemId = 1, Amount = 5 },
                new Crafting { CraftingId = 52, BlueprintId = 20, ItemId = 6, Amount = 10 },
                // Unobtainium Pickaxe: 5 Wood + 10 Unobtainium = 15 (MAX)
                new Crafting { CraftingId = 26, BlueprintId = 21, ItemId = 1, Amount = 5 },
                new Crafting { CraftingId = 27, BlueprintId = 21, ItemId = 7, Amount = 10 }
            );

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