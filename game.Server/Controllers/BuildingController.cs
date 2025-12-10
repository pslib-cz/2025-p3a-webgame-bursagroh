using Microsoft.AspNetCore.Mvc;
using game.Server.Models;
using game.Server.Data;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public BuildingController(ApplicationDbContext context)
        {
            this.context = context;
        }
    }
}