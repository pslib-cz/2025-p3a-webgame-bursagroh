using AutoMapper;
using game.Server.Data;
using game.Server.DTOs;
using game.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IRecipeService
{
    Task<ActionResult<List<RecipeDto>>> GetAllRecipesAsync();
    Task<ActionResult<RecipeDto>> GetRandomRecipeAsync();
    Task<ActionResult> StartRecipeAsync(int recipeId, StartRecipeRequest request);
    Task<ActionResult> EndRecipeAsync(int recipeId, EndRecipeRequest request);
    Task<ActionResult<List<RecipeTime>>> GetRecipeLeaderboardAsync();
}

public class RecipeService : IRecipeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RecipeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActionResult<List<RecipeDto>>> GetAllRecipesAsync()
    {
        var recipes = await _context.Recipes
            .Include(r => r.Ingrediences)
            .ToListAsync();

        if (recipes == null || !recipes.Any()) return new NoContentResult();

        var recipeDtos = _mapper.Map<List<RecipeDto>>(recipes);
        return new OkObjectResult(recipeDtos);
    }

    public async Task<ActionResult<RecipeDto>> GetRandomRecipeAsync()
    {
        var recipes = await _context.Recipes
            .Include(r => r.Ingrediences)
            .ToListAsync();

        if (recipes == null || !recipes.Any()) return new NoContentResult();

        var randomRecipe = recipes.OrderBy(r => Guid.NewGuid()).FirstOrDefault();
        var recipeDto = _mapper.Map<RecipeDto>(randomRecipe);

        return new OkObjectResult(recipeDto);
    }

    public async Task<ActionResult> StartRecipeAsync(int recipeId, StartRecipeRequest request)
    {
        var recipeExists = await _context.Recipes.AnyAsync(r => r.RecipeId == recipeId);

        if (!recipeExists) return new NotFoundObjectResult("Recept nenalezen.");

        var activeRecipeTime = await _context.RecipeTimes
            .FirstOrDefaultAsync(rt => rt.RecipeId == recipeId
                                    && rt.PlayerId == request.PlayerId
                                    && rt.EndTime == null);

        if (activeRecipeTime != null) return new ConflictObjectResult("uz to bezi");

        RecipeTime newRecipeTime = new RecipeTime
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
        Recipe? recipe = await _context.Recipes
            .Include(r => r.Ingrediences)
            .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

        if (recipe == null) return new NotFoundObjectResult("spatny rId");

        List<IngredienceTypes> correctOrder = recipe.Ingrediences
            .OrderBy(i => i.Order)
            .Select(i => i.IngredienceType)
            .ToList();

        List<IngredienceTypes> playerOrder = request.PlayerAssembly
            .Select(a => a.Type)
            .ToList();

        bool orderIsCorrect = correctOrder.Count == playerOrder.Count &&
                              correctOrder.SequenceEqual(playerOrder);

        if (!orderIsCorrect) return new BadRequestObjectResult("mas to spatne");

        RecipeTime? activeRecipeTime = await _context.RecipeTimes
            .Where(rt => rt.RecipeId == recipeId && rt.PlayerId == request.PlayerId)
            .OrderByDescending(rt => rt.StartTime)
            .FirstOrDefaultAsync(rt => rt.EndTime == null);

        if (activeRecipeTime == null) return new NotFoundObjectResult("nemuzes dat end bez startu");

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

        return new OkObjectResult(new
        {
            Duration = duration,
            player?.Money
        });
    }

    public async Task<ActionResult<List<RecipeTime>>> GetRecipeLeaderboardAsync()
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

        return new OkObjectResult(finalLeaderboard);
    }
}