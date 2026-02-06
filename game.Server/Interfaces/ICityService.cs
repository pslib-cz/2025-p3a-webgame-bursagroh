using game.Server.Requests;

public interface ICityService
{
    Task HandleCityMovement(Player player, MovePlayerRequest request, Guid id);
}