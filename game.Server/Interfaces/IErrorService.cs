using Microsoft.AspNetCore.Mvc;

namespace game.Server.Interfaces
{
    public interface IErrorService
    {
        ObjectResult CreateErrorResponse(int statusCode, int errorId, string message, string? heading = null, string? notificationText = null);
    }
}
