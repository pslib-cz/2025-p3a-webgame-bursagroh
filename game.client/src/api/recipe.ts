import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { IngredienceType } from "../types/api/models/recipe"

export const getRecipesQuery = () =>
    queryOptions({
        queryKey: ["recipes"],
        queryFn: () => api.get("/api/Recipe", {}, {}),
    })

export const getLeaderboardQuery = () =>
    queryOptions({
        queryKey: ["leaderboard"],
        queryFn: () => api.get("/api/Recipe/Leaderboard", {}, {}),
    })

export const getRandomRecipeMutation = (onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.get("/api/Recipe/Random", {}, {}),
        onError
    })

export const startRecipeMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: (recipeId: number) => api.patchWith204("/api/Recipe/{recipeId}/Action/start", { recipeId }, {}, { playerId }),
        onError
    })

const reassembleIngrediences = (ingrediences: Array<IngredienceType>) => {
    return ingrediences.map((ingredience) => ({type: ingredience}))
}

export const endRecipeMutation = ( playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: ({recipeId, playerAssembly}: {recipeId: number, playerAssembly: IngredienceType[]}) => api.patch("/api/Recipe/{recipeId}/Action/end", { recipeId }, {}, { playerId, playerAssembly: reassembleIngrediences(playerAssembly) }),
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: [playerId, "player"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: ["leaderboard"], refetchType: "active" })
            ])
        },
        onError
    })