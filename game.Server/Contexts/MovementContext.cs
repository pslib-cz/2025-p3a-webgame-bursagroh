using game.Server.Models;

public record MovementContext(
    HashSet<(int x, int y)> BlockedCoordinates,
    HashSet<(int x, int y)> EnemyCoordinates,
    List<FloorItem> StaticItems
);