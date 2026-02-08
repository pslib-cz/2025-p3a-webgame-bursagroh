using Microsoft.AspNetCore.Mvc;

public interface ISaveService
{
    Task<ActionResult> LoadSnapshotAsync(string saveString, Guid targetPlayerId);
    Task<ActionResult> ClonePlayerRecordAsync(Guid playerId);
    Task<ActionResult> DeletePlayerDataAsync(Guid playerId);

}