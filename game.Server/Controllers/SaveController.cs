using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SaveController : ControllerBase
{
    private readonly ISaveService _saveService;

    public SaveController(ISaveService saveService) => _saveService = saveService;

    [HttpPost("/api/Load")]
    public async Task<ActionResult> LoadSnapshot(string saveString, Guid targetPlayerId)
        => await _saveService.LoadSnapshotAsync(saveString, targetPlayerId);

    [HttpPost]
    public async Task<ActionResult> ClonePlayerRecord(Guid playerId)
        => await _saveService.ClonePlayerRecordAsync(playerId);
}