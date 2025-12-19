using game.Server.Models;

public class MapGeneratorService
{
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
                    IsBossDefeated = false
                });
            }
        }
        return buildings;
    }

    public List<Floor> GenerateInterior(int buildingId, int combinedSeed, int floorCount)
    {
        var floors = new List<Floor>();

        for (int i = 0; i < floorCount; i++)
        {
            Random rng = new Random(combinedSeed + i);

            var floor = new Floor
            {
                BuildingId = buildingId,
                Level = i,
                FloorItems = new List<FloorItem>()
            };

            floor.FloorItems.Add(new FloorItem
            {
                PositionX = 3,
                PositionY = 3,
                FloorItemType = FloorItemType.Stair
            });

            int chestCount = rng.Next(1, 4);
            for (int c = 0; c < chestCount; c++)
            {
                var item = new FloorItem
                {
                    PositionX = rng.Next(0, 8),
                    PositionY = rng.Next(0, 8),
                    FloorItemType = FloorItemType.Chest,
                    Chest = new Chest()
                };
                floor.FloorItems.Add(item);
            }

            int enemyCount = rng.Next(2, 5); 
            for (int e = 0; e < enemyCount; e++)
            {
                var types = Enum.GetValues<EnemyType>();
                var selectedType = (EnemyType)types.GetValue(rng.Next(types.Length));

                var item = new FloorItem
                {
                    PositionX = rng.Next(0, 8),
                    PositionY = rng.Next(0, 8),
                    FloorItemType = FloorItemType.Enemy,
                    Enemy = new Enemy
                    {
                        Health = selectedType == EnemyType.Dragon ? 100 : 20,
                    }
                };
                floor.FloorItems.Add(item);
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
        return BuildingTypes.Abandoned;
    }
}