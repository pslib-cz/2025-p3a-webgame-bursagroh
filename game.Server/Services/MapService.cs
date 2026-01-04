using game.Server.Models;

public class MapGeneratorService
{
    private readonly Random _rng = new Random();
    public MapGeneratorService() { }

    public List<Building> GenerateMapArea(Guid playerId, int minX, int maxX, int minY, int maxY)
    {
        var buildings = new List<Building>();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                BuildingTypes type = DetermineBuildingType(x, y);
                int absX = Math.Abs(x);
                int absY = Math.Abs(y);

                if (absX % 4 == 3 && absY % 4 == 3) continue;
                if ((x == 0 || y == 0) && type == BuildingTypes.Abandoned) continue;

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

    public List<Floor> GenerateInterior(int buildingId, int combinedSeed, int floorCount, int totalBuildingHeight)
    {
        var floors = new List<Floor>();

        for (int i = 0; i < floorCount; i++)
        {
            Random rng = new Random(combinedSeed + i);

            bool isFirstFloor = (i == 0);
            bool isRealLastFloor = (i == totalBuildingHeight - 1);

            var occupiedPositions = new HashSet<string>();

            var floor = new Floor
            {
                BuildingId = buildingId,
                Level = i,
                FloorItems = new List<FloorItem>()
            };

            (int x, int y) GetFreePos()
            {
                int nx, ny;
                do
                {
                    nx = rng.Next(0, 8);
                    ny = rng.Next(0, 8);
                } while (occupiedPositions.Contains($"{nx},{ny}"));

                occupiedPositions.Add($"{nx},{ny}");
                return (nx, ny);
            }

            if (!isFirstFloor)
            {
                floor.FloorItems.Add(new FloorItem { PositionX = 2, PositionY = 2, FloorItemType = FloorItemType.Stair });
                occupiedPositions.Add("2,2");
            }

            if (!isRealLastFloor)
            {
                floor.FloorItems.Add(new FloorItem { PositionX = 5, PositionY = 2, FloorItemType = FloorItemType.Stair });
                occupiedPositions.Add("5,2");
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

            if (isRealLastFloor)
            {
                var pos = GetFreePos();
                floor.FloorItems.Add(new FloorItem
                {
                    PositionX = pos.x,
                    PositionY = pos.y,
                    FloorItemType = FloorItemType.Enemy,
                    Enemy = new Enemy { Health = 1000, EnemyType = EnemyType.Dragon }
                });
            }

            int enemyCount = isRealLastFloor ? rng.Next(1, 3) : rng.Next(2, 5);
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
                    Enemy = new Enemy { Health = 200, EnemyType = selectedType }
                });
            }

            floors.Add(floor);
        }

        return floors;
    }

    private BuildingTypes DetermineBuildingType(int x, int y)
    {
        if (Math.Abs(x) % 4 == 1 || Math.Abs(y) % 4 == 1)
        {
            return BuildingTypes.Road;
        }

        if (_rng.NextDouble() < 0.10)
        {
            return BuildingTypes.AbandonedTrap;
        }

        return BuildingTypes.Abandoned;
    }
}