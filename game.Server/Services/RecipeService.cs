using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Interfaces;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class RecipeService : IRecipeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IErrorService _errorService; // Added Error Service

    public RecipeService(ApplicationDbContext context, IMapper mapper, IErrorService errorService)
    {
        _context = context;
        _mapper = mapper;
        _errorService = errorService;
    }

    public async Task<ActionResult<List<RecipeDto>>> GetAllRecipesAsync()
    {
        var recipes = await _context.Recipes
            .Include(r => r.Ingrediences)
            .ToListAsync();

        if (recipes == null || !recipes.Any()) return new NoContentResult();

        return new OkObjectResult(_mapper.Map<List<RecipeDto>>(recipes));
    }

    public async Task<ActionResult<RecipeDto>> GetRandomRecipeAsync()
    {
        var recipes = await _context.Recipes
            .Include(r => r.Ingrediences)
            .ToListAsync();

        if (recipes == null || !recipes.Any()) return new NoContentResult();

        var randomRecipe = recipes.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
        return new OkObjectResult(_mapper.Map<RecipeDto>(randomRecipe));
    }

    public async Task<ActionResult> StartRecipeAsync(int recipeId, StartRecipeRequest request)
    {
        var recipeExists = await _context.Recipes.AnyAsync(r => r.RecipeId == recipeId);
        if (!recipeExists)
        {
            return _errorService.CreateErrorResponse(404, 9001, "Recipe not found.", "Not Found");
        }

        var activeRecipeTime = await _context.RecipeTimes
            .FirstOrDefaultAsync(rt => rt.RecipeId == recipeId
                                    && rt.PlayerId == request.PlayerId
                                    && rt.EndTime == null);

        if (activeRecipeTime != null)
        {
            return _errorService.CreateErrorResponse(409, 9002, "A session for this recipe is already active.", "Conflict");
        }

        var newRecipeTime = new RecipeTime
        {
            RecipeId = recipeId,
            PlayerId = request.PlayerId,
            StartTime = DateTime.UtcNow,
            EndTime = null
        };

        _context.RecipeTimes.Add(newRecipeTime);
        await _context.SaveChangesAsync();

        return new NoContentResult();
    }

    public async Task<ActionResult> EndRecipeAsync(int recipeId, EndRecipeRequest request)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingrediences)
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

        if (recipe == null)
        {
            return _errorService.CreateErrorResponse(404, 9001, "Recipe not found.", "Not Found");
        }

        var correctOrder = recipe.Ingrediences.OrderBy(i => i.Order).Select(i => i.IngredienceType).ToList();
        var playerOrder = request.PlayerAssembly.Select(a => a.Type).ToList();

        if (correctOrder.Count != playerOrder.Count || !correctOrder.SequenceEqual(playerOrder))
        {
            return _errorService.CreateErrorResponse(400, 9003, "Incorrect ingredient sequence.", "Validation Failed");
        }

        var activeRecipeTime = await _context.RecipeTimes
            .Where(rt => rt.RecipeId == recipeId && rt.PlayerId == request.PlayerId)
            .OrderByDescending(rt => rt.StartTime)
            .FirstOrDefaultAsync(rt => rt.EndTime == null);

        if (activeRecipeTime == null)
        {
            return _errorService.CreateErrorResponse(400, 9004, "No active recipe session found to end.", "Action Denied");
        }

        activeRecipeTime.EndTime = DateTime.UtcNow;

        var player = await _context.Players.FindAsync(request.PlayerId);
        if (player != null)
        {
            player.Money += 20;
        }

        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            Duration = activeRecipeTime.EndTime - activeRecipeTime.StartTime,
            player?.Money
        });
    }

    public async Task<ActionResult<List<LeaderboardDto>>> GetRecipeLeaderboardAsync()
    {
        List<RecipeTime> completedTimes = await _context.RecipeTimes
            .Where(rt => rt.EndTime > rt.StartTime)
            .ToListAsync();

        if (completedTimes == null || completedTimes.Count == 0) return new NoContentResult();

        List<RecipeTime> validTimes = completedTimes
            .Where(rt => rt.Duration > 0.01)
            .ToList();

        if (validTimes.Count == 0) return new NoContentResult();

        List<RecipeTime> bestTimePerRecipe = validTimes
            .GroupBy(rt => rt.RecipeId)
            .Select(group => group.OrderBy(rt => rt.Duration).First())
            .ToList();

        List<RecipeTime> finalLeaderboard = bestTimePerRecipe
            .OrderBy(rt => rt.Duration)
            .ToList();

        // This will now work if the Profile ignores or manually maps 'Player'
        return new OkObjectResult(_mapper.Map<List<LeaderboardDto>>(finalLeaderboard));
    }
}
