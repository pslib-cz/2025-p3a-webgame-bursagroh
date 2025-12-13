using game.Server.Data;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RecipeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetAllRecipes()
        {
            List<Recipe> recipes = await context.Recipes.Include(r => r.Ingrediences).ToListAsync();

            if (recipes == null || recipes.Count == 0)
            {
                return NoContent();
            }

            return Ok(recipes);
        }

        [HttpGet("Random")]
        public async Task<ActionResult<List<Recipe>>> GetRandomRecipe()
        {
            List<Recipe> recipes = await context.Recipes.Include(r => r.Ingrediences).ToListAsync();

            if (recipes == null || recipes.Count == 0)
            {
                return NoContent();
            }

            Recipe? randomRecipe = recipes.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
            return Ok(randomRecipe);
        }

        [HttpPatch("{recipeId}/Action/start")]
        public async Task<ActionResult> StartRecipe(int recipeId, [FromBody] StartRecipeRequest request)
        {
            var recipe = await context.Recipes.AnyAsync(r => r.RecipeId == recipeId);

            if (!recipe)
            {
                return NotFound();
            }

            RecipeTime newRecipeTime = new RecipeTime
            {
                RecipeId = recipeId,
                PlayerId = request.PlayerId,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow
            };

            context.RecipeTimes.Add(newRecipeTime);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{recipeId}/Action/end")]
        public async Task<ActionResult> EndRecipe(int recipeId, [FromBody] EndRecipeRequest request)
        {
            Recipe? recipe = await context.Recipes
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

            RecipeTime? activeRecipeTime = await context.RecipeTimes
                .Where(rt => rt.RecipeId == recipeId && rt.PlayerId == request.PlayerId)
                .OrderByDescending(rt => rt.StartTime)
                .FirstOrDefaultAsync(rt => rt.StartTime == rt.EndTime); 

            if (activeRecipeTime == null)
            {
                return NotFound("nemuzes dat end bez startu");
            }

            DateTime endTime = DateTime.UtcNow;
            activeRecipeTime.EndTime = endTime;
            await context.SaveChangesAsync();

            TimeSpan duration = activeRecipeTime.EndTime - activeRecipeTime.StartTime;

            return Ok(new 
            { 
                DurationSeconds = duration.TotalSeconds
            });
        }

        [HttpGet("AllTimes")]
        public async Task<ActionResult<List<RecipeTime>>> GetAllRecipeTimes()
        {

            var recipeTimes = await context.RecipeTimes.ToListAsync();

            if (recipeTimes == null || recipeTimes.Count == 0)
            {
                return NoContent();
            }

            return Ok(recipeTimes);
        }

        [HttpGet("Leaderboard")]
        public async Task<ActionResult<List<RecipeTime>>> GetRecipeLeaderboard()
        {
            List<RecipeTime> completedTimes = await context.RecipeTimes
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