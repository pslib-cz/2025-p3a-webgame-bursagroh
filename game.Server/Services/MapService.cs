using game.Server.Types;
using game.Server.Models;

public class MapGeneratorService
{
    private const int GridSize = 8;
    private const int MaxSpawnAttempts = 100;

    public MapGeneratorService() { }

    public static bool IsRoad(int x, int y)
    {
        return Math.Abs(x) % 4 == 1 || Math.Abs(y) % 4 == 1;
    }

    public static List<(int x, int y)> GetExitCoordinates(int bX, int bY)
    {
        List<(int, int)> exits = new List<(int x, int y)>();

        if (IsRoad(bX, bY - 1)) { exits.Add((3, 0)); exits.Add((4, 0)); }
        if (IsRoad(bX, bY + 1)) { exits.Add((3, 7)); exits.Add((4, 7)); }
        if (IsRoad(bX - 1, bY)) { exits.Add((0, 3)); exits.Add((0, 4)); }
        if (IsRoad(bX + 1, bY)) { exits.Add((7, 3)); exits.Add((7, 4)); }

        if (exits.Count == 0) exits.Add((0, 0));

        return exits;
    }

    public List<Building> GenerateMapArea(Guid playerId, int minX, int maxX, int minY, int maxY, int globalSeed)
    {
        var buildings = new List<Building>();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                int coordinateSeed = HashCode.Combine(globalSeed, x, y);
                Random coordRng = new Random(coordinateSeed);

                BuildingTypes type = DetermineBuildingType(x, y, coordRng);

                if (Math.Abs(x) % 4 == 3 && Math.Abs(y) % 4 == 3) continue;
                if ((x == 0 || y == 0) && IsHostileBuilding(type)) continue;

                buildings.Add(new Building
                {
                    PlayerId = playerId,
                    BuildingType = type,
                    PositionX = x,
                    PositionY = y,
                    Height = IsHostileBuilding(type) ? coordRng.Next(5, 10) : null,
                    ReachedHeight = IsHostileBuilding(type) ? 0 : null,
                    IsBossDefeated = IsHostileBuilding(type) ? false : null
                });
            }
        }
        return buildings;
    }

    public List<Floor> GenerateInterior(int buildingId, int globalSeed, int floorCount, int totalHeight, int bX, int bY)
    {
        var floors = new List<Floor>();
        int buildingSeed = HashCode.Combine(globalSeed, bX, bY);

        for (int i = 0; i < floorCount; i++)
        {
            var floorRng = new Random(buildingSeed + i);
            floors.Add(GenerateSingleFloor(buildingId, i, totalHeight, bX, bY, floorRng));
        }
        return floors;
    }

    private Floor GenerateSingleFloor(int buildingId, int level, int totalHeight, int bX, int bY, Random rng)
    {
        var floor = new Floor
        {
            BuildingId = buildingId,
            Level = level,
            FloorItems = new List<FloorItem>()
        };

        var occupiedPositions = new HashSet<(int x, int y)>();
        PlaceFixedFeatures(floor, level, totalHeight, bX, bY, occupiedPositions);
        PlaceChests(floor, rng, occupiedPositions);
        PlaceEnemies(floor, level, totalHeight, rng, occupiedPositions);

        return floor;
    }

    private void PlaceFixedFeatures(Floor floor, int level, int totalHeight, int bX, int bY, HashSet<(int x, int y)> occupied)
    {
        bool isEvenFloor = (level % 2 == 0);
        bool isLastFloor = (level == totalHeight - 1);

        (int x, int y) downStairs = isEvenFloor ? (2, 2) : (5, 2);
        (int x, int y) upStairs = isEvenFloor ? (5, 2) : (2, 2);

        if (level == 0)
        {
            foreach (var exit in GetExitCoordinates(bX, bY))
            {
                occupied.Add((exit.x, exit.y));
            }
        }

        if (level > 0)
        {
            occupied.Add(downStairs);
            floor?.FloorItems?.Add(new FloorItem { PositionX = downStairs.x, PositionY = downStairs.y, FloorItemType = FloorItemType.Stair });
        }

        if (!isLastFloor)
        {
            occupied.Add(upStairs);
            floor?.FloorItems?.Add(new FloorItem { PositionX = upStairs.x, PositionY = upStairs.y, FloorItemType = FloorItemType.Stair });
        }
    }

    private void PlaceChests(Floor floor, Random rng, HashSet<(int x, int y)> occupied)
    {
        int count = rng.Next(1, 4);
        for (int i = 0; i < count; i++)
        {
            if (TryGetFreePosition(rng, occupied, out var pos))
            {
                floor?.FloorItems?.Add(new FloorItem
                {
                    PositionX = pos.x,
                    PositionY = pos.y,
                    FloorItemType = FloorItemType.Chest,
                    Chest = new Chest()
                });
            }
        }
    }

    private void PlaceEnemies(Floor floor, int level, int totalHeight, Random rng, HashSet<(int x, int y)> occupied)
    {
        bool isLastFloor = (level == totalHeight - 1);

        if (isLastFloor && TryGetFreePosition(rng, occupied, out var bossPos))
        {
            floor?.FloorItems?.Add(new FloorItem
            {
                PositionX = bossPos.x,
                PositionY = bossPos.y,
                FloorItemType = FloorItemType.Enemy,
                Enemy = new Enemy { Health = 100, MaxHealth = 100, EnemyType = EnemyType.Dragon }
            });
        }

        int mobCount = isLastFloor ? rng.Next(1, 3) : rng.Next(2, 5);
        var enemyTypes = Enum.GetValues<EnemyType>().Where(t => t != EnemyType.Dragon).ToList();

        for (int i = 0; i < mobCount; i++)
        {
            if (TryGetFreePosition(rng, occupied, out var pos))
            {
                var type = enemyTypes[rng.Next(enemyTypes.Count)];
                floor?.FloorItems?.Add(new FloorItem
                {
                    PositionX = pos.x,
                    PositionY = pos.y,
                    FloorItemType = FloorItemType.Enemy,
                    Enemy = new Enemy
                    {
                        Health = 20,
                        MaxHealth = 20,
                        EnemyType = type,
                        ItemInstance = new ItemInstance { ItemId = 10, Durability = 20 }
                    }
                });
            }
        }
    }

    private bool TryGetFreePosition(Random rng, HashSet<(int x, int y)> occupied, out (int x, int y) position)
    {
        for (int i = 0; i < MaxSpawnAttempts; i++)
        {
            int nx = rng.Next(0, GridSize);
            int ny = rng.Next(0, GridSize);

            if (!occupied.Contains((nx, ny)))
            {
                occupied.Add((nx, ny));
                position = (nx, ny);
                return true;
            }
        }
        position = (-1, -1);
        return false;
    }

    private BuildingTypes DetermineBuildingType(int x, int y, Random coordRng)
    {
        if (IsRoad(x, y)) return BuildingTypes.Road;
        return coordRng.NextDouble() < 0.10 ? BuildingTypes.AbandonedTrap : BuildingTypes.Abandoned;
    }

    private bool IsHostileBuilding(BuildingTypes type) => type == BuildingTypes.Abandoned || type == BuildingTypes.AbandonedTrap;
}