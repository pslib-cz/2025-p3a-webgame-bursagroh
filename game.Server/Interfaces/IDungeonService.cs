using game.Server.Models;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;

public interface IDungeonService
{
    Task<ActionResult?> HandleInternalLogic(Player player, Mine? playerMine, MovePlayerRequest request);
}