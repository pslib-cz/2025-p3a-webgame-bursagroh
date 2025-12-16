using game.Server.Models;

public class MapGeneratorService
{
    private readonly int MapRadius;

    public MapGeneratorService(int radius = 4)
    {
        this.MapRadius = radius;
    }

    public List<Building> GenerateMapExtensions(Guid playerId)
    {
        var buildings = new List<Building>();

        for (int x = -MapRadius; x <= MapRadius; x++)
        {
            for (int y = -MapRadius; y <= MapRadius; y++)
            {
                BuildingTypes type = DetermineBuildingType(x, y);
                int absX = Math.Abs(x);
                int absY = Math.Abs(y);

                if (absX % 4 == 3 && absY % 4 == 3)
                {
                    continue;
                }

                if ((x == 0 || y == 0) && type == BuildingTypes.Abandoned)
                {
                    continue;
                }

                buildings.Add(new Building
                {
                    PlayerId = playerId,
                    BuildingType = type,
                    PositionX = x,
                    PositionY = y,
                    IsBossDefeated = false,
                    Height = null,
                    ReachedHeight = null,
                    Floors = null
                });
            }
        }
        return buildings;
    }


    private BuildingTypes DetermineBuildingType(int x, int y)
    {
        int absX = Math.Abs(x);
        int absY = Math.Abs(y);
        if (absX % 4 == 1 || absY % 4 == 1)
        {
            return BuildingTypes.Road;
        }

        return BuildingTypes.Abandoned;
    }
}