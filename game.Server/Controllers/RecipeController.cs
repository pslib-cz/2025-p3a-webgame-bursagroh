using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RecipeController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecipeDto>>> GetAllRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Ingrediences)
                .ToListAsync();

            if (recipes == null || !recipes.Any())
            {
                return NoContent();
            }

            var recipeDtos = _mapper.Map<List<RecipeDto>>(recipes);

            return Ok(recipeDtos);
        }

        [HttpGet("Random")]
        public async Task<ActionResult<RecipeDto>> GetRandomRecipe()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Ingrediences)
                .ToListAsync();

            if (recipes == null || !recipes.Any())
            {
                return NoContent();
            }

            var randomRecipe = recipes.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            var recipeDto = _mapper.Map<RecipeDto>(randomRecipe);

            return Ok(recipeDto);
        }

        [HttpPatch("{recipeId}/Action/start")]
        public async Task<ActionResult> StartRecipe(int recipeId, [FromBody] StartRecipeRequest request)
        {
            var recipeExists = await _context.Recipes.AnyAsync(r => r.RecipeId == recipeId);

            if (!recipeExists)
            {
                return NotFound("Recept nenalezen.");
            }

            var activeRecipeTime = await _context.RecipeTimes
                .FirstOrDefaultAsync(rt => rt.RecipeId == recipeId
                                        && rt.PlayerId == request.PlayerId
                                        && rt.EndTime == null); 

            if (activeRecipeTime != null)
            {
                return Conflict("uz to bezi");
            }
            RecipeTime newRecipeTime = new RecipeTime
            {
                RecipeId = recipeId,
                PlayerId = request.PlayerId,
                StartTime = DateTime.UtcNow,
                EndTime = null
            };

            _context.RecipeTimes.Add(newRecipeTime);
            await _context.SaveChangesAsync();


            return NoContent();
        }

        [HttpPatch("{recipeId}/Action/end")]
        public async Task<ActionResult> EndRecipe(int recipeId, [FromBody] EndRecipeRequest request)
        {
            Recipe? recipe = await _context.Recipes
                .Include(r => r.Ingrediences)
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
            {
                return NotFound("spatny rId");
            }
    
            List<IngredienceTypes> correctOrder = recipe.Ingrediences
                .OrderBy(i => i.Order)
                .Select(i => i.IngredienceType)
                .ToList();

            List<IngredienceTypes> playerOrder = request.PlayerAssembly
                .Select(a => a.Type)
                .ToList();

            bool orderIsCorrect = correctOrder.Count == playerOrder.Count &&
                                  correctOrder.SequenceEqual(playerOrder);

            if (!orderIsCorrect)
            {
                return BadRequest("mas to spatne");
            }

            RecipeTime? activeRecipeTime = await _context.RecipeTimes
                .Where(rt => rt.RecipeId == recipeId && rt.PlayerId == request.PlayerId)
                .OrderByDescending(rt => rt.StartTime)
                .FirstOrDefaultAsync(rt => rt.EndTime == null); 

            if (activeRecipeTime == null)
            {
                return NotFound("nemuzes dat end bez startu");
            }

            DateTime endTime = DateTime.UtcNow;
            activeRecipeTime.EndTime = endTime;
            await _context.SaveChangesAsync();

            TimeSpan? duration = activeRecipeTime.EndTime - activeRecipeTime.StartTime;

            Player? player = await _context.Players.FindAsync(request.PlayerId);
            if (player != null)
            {

                player.Money += 20;
                await _context.SaveChangesAsync();
            }

            return Ok(new 
            { 
                Duration = duration,
                player.Money
            });
        }

        [HttpGet("Leaderboard")]
        public async Task<ActionResult<List<RecipeTime>>> GetRecipeLeaderboard()
        {
            List<RecipeTime> completedTimes = await _context.RecipeTimes
                .Where(rt => rt.EndTime > rt.StartTime)
                .ToListAsync();

            if (completedTimes == null || completedTimes.Count == 0)
            {
                return NoContent();
            }

            List<RecipeTime> validTimes = completedTimes
                .Where(rt => rt.Duration > 0.01)
                .ToList();

            if (validTimes.Count == 0)
            {
                return NoContent();
            }

            List<RecipeTime> bestTimePerRecipe = validTimes
                .GroupBy(rt => rt.RecipeId)
                .Select(group => group.OrderBy(rt => rt.Duration).First())
                .ToList();

            List<RecipeTime> finalLeaderboard = bestTimePerRecipe
                .OrderBy(rt => rt.Duration)
                .ToList();

            return Ok(finalLeaderboard);
        }
    }
}