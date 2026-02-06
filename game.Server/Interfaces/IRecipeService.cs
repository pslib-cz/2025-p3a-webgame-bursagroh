using game.Server.DTOs;
using game.Server.Requests;
using Microsoft.AspNetCore.Mvc;

public interface IRecipeService
{
    Task<ActionResult<List<RecipeDto>>> GetAllRecipesAsync();
    Task<ActionResult<RecipeDto>> GetRandomRecipeAsync();
    Task<ActionResult> StartRecipeAsync(int recipeId, StartRecipeRequest request);
    Task<ActionResult> EndRecipeAsync(int recipeId, EndRecipeRequest request);
    Task<ActionResult<List<LeaderboardDto>>> GetRecipeLeaderboardAsync();
}