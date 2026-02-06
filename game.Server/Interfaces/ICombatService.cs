using Microsoft.AspNetCore.Mvc;

public interface ICombatService
{
    Task<ActionResult> UseItemAsync(Guid id);
}