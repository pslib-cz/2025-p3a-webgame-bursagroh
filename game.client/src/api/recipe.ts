import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { IngredienceType } from "../types/api/models/recipe"

export const getRecipesQuery = () =>
    queryOptions({
        queryKey: ["recipes"],
        queryFn: () => api.get("/api/Recipe", {}, {}),
    })

export const getRandomRecipeMutation = () =>
    mutationOptions({
        mutationFn: () => api.get("/api/Recipe/Random", {}, {}),
    })

export const startRecipeMutation = (playerId: string) =>
    mutationOptions({
        mutationFn: (recipeId: number) => api.patchWith204("/api/Recipe/{recipeId}/Action/start", { recipeId }, {}, { playerId }),
    })

const reassembleIngrediences = (ingrediences: Array<IngredienceType>) => {
    return ingrediences.map((ingredience) => ({type: ingredience}))
}

export const endRecipeMutation = ( playerId: string) =>
    mutationOptions({
        mutationFn: ({recipeId, playerAssembly}: {recipeId: number, playerAssembly: IngredienceType[]}) => api.patch("/api/Recipe/{recipeId}/Action/end", { recipeId }, {}, { playerId, playerAssembly: reassembleIngrediences(playerAssembly) }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
        },
    })