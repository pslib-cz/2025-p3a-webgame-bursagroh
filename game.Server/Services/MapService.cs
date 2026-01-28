using game.Server.Models;

public class MapGeneratorService
{
    public MapGeneratorService() { }

    public static bool IsRoad(int x, int y)
    {
        return Math.Abs(x) % 4 == 1 || Math.Abs(y) % 4 == 1;
    }

    public static List<(int x, int y)> GetExitCoordinates(int bX, int bY)
    {
        var exits = new List<(int x, int y)>();

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
                int absX = Math.Abs(x);
                int absY = Math.Abs(y);

                if (absX % 4 == 3 && absY % 4 == 3) continue;
                if ((x == 0 || y == 0) && (type == BuildingTypes.Abandoned || type == BuildingTypes.AbandonedTrap)) continue;

                buildings.Add(new Building
                {
                    PlayerId = playerId,
                    BuildingType = type,
                    PositionX = x,
                    PositionY = y,
                    Height = (type == BuildingTypes.Abandoned || type == BuildingTypes.AbandonedTrap) ? 5 : null,
                    ReachedHeight = (type == BuildingTypes.Abandoned || type == BuildingTypes.AbandonedTrap) ? 0 : null,
                    IsBossDefeated = (type == BuildingTypes.Abandoned || type == BuildingTypes.AbandonedTrap) ? false : null
                });
            }
        }
        return buildings;
    }

    public List<Floor> GenerateInterior(int buildingId, int globalSeed, int floorCount, int totalBuildingHeight, int bX, int bY)
    {
        var floors = new List<Floor>();
        int buildingSeed = HashCode.Combine(globalSeed, bX, bY);

        for (int i = 0; i < floorCount; i++)
        {
            Random rng = new Random(buildingSeed + i);
            bool isEvenFloor = (i % 2 == 0);
            bool isRealLastFloor = (i == totalBuildingHeight - 1);

            var occupiedPositions = new HashSet<string>();
            var floor = new Floor
            {
                BuildingId = buildingId,
                Level = i,
                FloorItems = new List<FloorItem>()
            };

            int downX = isEvenFloor ? 2 : 5;
            int upX = isEvenFloor ? 5 : 2;
            int stairY = 2;

            occupiedPositions.Add($"{downX},{stairY}");
            occupiedPositions.Add($"{upX},{stairY}");

            if (i > 0)
            {
                floor.FloorItems.Add(new FloorItem { PositionX = downX, PositionY = stairY, FloorItemType = FloorItemType.Stair });
            }
            else
            {
                var exits = GetExitCoordinates(bX, bY);
                foreach (var exit in exits)
                {
                    occupiedPositions.Add($"{exit.x},{exit.y}");
                }
                    
            }

            if (!isRealLastFloor)
            {
                floor.FloorItems.Add(new FloorItem { PositionX = upX, PositionY = stairY, FloorItemType = FloorItemType.Stair });
            }

            (int x, int y) GetFreePos()
            {
                int nx, ny;
                int attempts = 0;
                do
                {
                    nx = rng.Next(0, 8);
                    ny = rng.Next(0, 8);
                    attempts++;
                } while (occupiedPositions.Contains($"{nx},{ny}") && attempts < 100);

                occupiedPositions.Add($"{nx},{ny}");
                return (nx, ny);
            }

            int chestCount = rng.Next(1, 4);
            for (int c = 0; c < chestCount; c++)
            {
                var pos = GetFreePos();
                floor.FloorItems.Add(new FloorItem
                {
                    PositionX = pos.x,
                    PositionY = pos.y,
                    FloorItemType = FloorItemType.Chest,
                    Chest = new Chest()
                });
            }

            int enemyCount = isRealLastFloor ? rng.Next(1, 3) : rng.Next(2, 5);
            if (isRealLastFloor)
            {
                var pos = GetFreePos();
                floor.FloorItems.Add(new FloorItem
                {
                    PositionX = pos.x,
                    PositionY = pos.y,
                    FloorItemType = FloorItemType.Enemy,
                    Enemy = new Enemy { Health = 100, EnemyType = EnemyType.Dragon }
                });
            }

            var randomEnemyPool = Enum.GetValues<EnemyType>().Where(t => t != EnemyType.Dragon).ToList();
            for (int e = 0; e < enemyCount; e++)
            {
                var pos = GetFreePos();
                var selectedType = randomEnemyPool[rng.Next(randomEnemyPool.Count)];
                floor.FloorItems.Add(new FloorItem
                {
                    PositionX = pos.x,
                    PositionY = pos.y,
                    FloorItemType = FloorItemType.Enemy,
                    Enemy = new Enemy { Health = 20, EnemyType = selectedType, ItemInstance = new ItemInstance { ItemId = 10, Durability = 20 } }
                });
            }

            floors.Add(floor);
        }
        return floors;
    }

    private BuildingTypes DetermineBuildingType(int x, int y, Random coordRng)
    {
        if (IsRoad(x, y)) return BuildingTypes.Road;

        if (coordRng.NextDouble() < 0.10)
        {
            return BuildingTypes.AbandonedTrap;
        }

        return BuildingTypes.Abandoned;
    }
}