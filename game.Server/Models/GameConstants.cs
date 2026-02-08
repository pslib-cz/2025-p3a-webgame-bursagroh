namespace game.Server.Models
{
    public class GameConstants
    {
        // Mine Config
        public const int MineExitX = 4;
        public const int MineExitY = -3;
        public const int MineMaxX = 7;
        public const int MineMinY = -2;
        public const int InitialMineGenerationDepth = 1;
        public const int InitialMineGenerationRange = 5;

        // Shop / Economy
        public const int PickaxeShopX1 = 1;
        public const int PickaxeShopX2 = 2;
        public const int PickaxeShopY = -2;
        public const int WoodenPickaxePrice = 5;

        public const int ItemIdWood = 1;
        public const int ItemIdWoodenPickaxe = 39;

        public const int AxeDamageMultiplier = 2;
        public const int DefaultMiningRange = 1;

        public const int GridSize = 8;
        public const int FloorMaxX = 7;
        public const int FloorMaxY = 7;
        public const int FloorMidLower = 3;
        public const int FloorMidUpper = 4;

        public const int MaxSpawnAttempts = 100;
        public const double TrapBuildingChance = 0.10;
        public const int RoadFrequency = 4;
        public const int RoadOffset = 1;
        public const int VoidTileMod = 3;

        public const int MinBuildingHeight = 5;
        public const int MaxBuildingHeight = 10;
        public const int DefaultBuildingHeight = 5;

        public const int MinChestsPerFloor = 1;
        public const int MaxChestsPerFloor = 4;
        public const int MinEnemiesPerFloor = 2;
        public const int MaxEnemiesPerFloor = 5;
        public const int MinEnemiesBossFloor = 1;
        public const int MaxEnemiesBossFloor = 3;

        public const int BossHealth = 100;
        public const int MobHealth = 20;
        public const int DefaultMobItemId = 10;
        public const int DefaultMobDurability = 20;

        public const int StairAX = 2;
        public const int StairBX = 5;
        public const int StairY = 2;

        public static readonly int[] ChestLoot = { 10, 20, 30, 11, 21, 31, 12, 22, 32, 40, 41, 42 };

        public const int FountainX = 0;
        public const int FountainY = 0;
        public const int MineEntranceX = 2;
        public const int MineEntranceY = 0;
        public const int BankX = -2;
        public const int BankY = 0;
        public const int RestaurantX = 0;
        public const int RestaurantY = -2;
        public const int BlacksmithX = 0;
        public const int BlacksmithY = 2;

        public const string ProtectedPlayerId = "4b1e8a93-7d92-4f7f-80c1-525c345b85e0";
    }
}