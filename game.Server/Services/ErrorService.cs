using game.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace game.Server.Services
{
    public class ErrorService : IErrorService
    {
        public ObjectResult CreateErrorResponse(int statusCode, int errorId, string message, string? heading = null, string? notificationText = null)
        {
            var apiError = new
            {
                statusCode,
                errorId,
                message,
                notification = heading != null ? new { heading, text = message } : null
            };

            return new ObjectResult(apiError) { StatusCode = statusCode };
        }
    }
}
