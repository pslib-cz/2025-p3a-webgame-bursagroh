using game.Server.DTOs;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("per_user_limit")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipeController(IRecipeService recipeService) => _recipeService = recipeService;

    [HttpGet]
    public async Task<ActionResult<List<RecipeDto>>> GetAllRecipes()
        => await _recipeService.GetAllRecipesAsync();

    [HttpGet("Random")]
    public async Task<ActionResult<RecipeDto>> GetRandomRecipe()
        => await _recipeService.GetRandomRecipeAsync();

    [HttpPatch("{recipeId}/Action/start")]
    public async Task<ActionResult> StartRecipe(int recipeId, [FromBody] StartRecipeRequest request)
        => await _recipeService.StartRecipeAsync(recipeId, request);

    [HttpPatch("{recipeId}/Action/end")]
    public async Task<ActionResult> EndRecipe(int recipeId, [FromBody] EndRecipeRequest request)
        => await _recipeService.EndRecipeAsync(recipeId, request);

    [HttpGet("Leaderboard")]
    public async Task<ActionResult<List<RecipeTime>>> GetRecipeLeaderboard()
        => await _recipeService.GetRecipeLeaderboardAsync();
}