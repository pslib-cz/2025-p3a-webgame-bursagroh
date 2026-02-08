using game.Server.DTOs;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;

public interface IPlayerService
{
    Task<ActionResult<PlayerDto>> GeneratePlayer(GeneratePlayerRequest request);
    Task<ActionResult<PlayerDto>> GetPlayerAsync(Guid id);
    Task<ActionResult<PlayerDto>> MoveScreenAsync(Guid id, MoveScreenRequest request);
    Task<ActionResult<PlayerDto>> MovePlayerAsync(Guid id, MovePlayerRequest request);
    Task<ActionResult<PlayerDto>> RenamePlayerAsync(Guid id, RenamePlayerRequest request);
}
