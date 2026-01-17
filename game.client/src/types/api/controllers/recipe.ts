import type { GenericGET, GenericPATCH } from ".."
import type { IngredienceType, Recipe, RecipeResult } from "../models/recipe"

export interface APIGetRecipes extends GenericGET {
    res: {
        200: Array<Recipe>
    }
}

export interface APIGetRandomRecipe extends GenericGET {
    res: {
        200: Recipe
    }
}

export interface APIRecipeStart extends GenericPATCH {
    params: {
        recipeId: number
    }
    body: {
        playerId: string
    }
    res: {
        200: object
        204: object
    }
}

export interface APIRecipeEnd extends GenericPATCH {
    params: {
        recipeId: number
    }
    body: {
        playerId: string
        playerAssembly: Array<{ type: IngredienceType }>
    }
    res: {
        200: RecipeResult
    }
}