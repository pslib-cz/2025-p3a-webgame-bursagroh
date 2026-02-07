import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."

export const getBlueprintsQuery = () =>
    queryOptions({
        queryKey: ["blueprint"],
        queryFn: () => api.get("/api/Blueprint", {}, {}),
    })

export const getPlayerBlueprintsQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "blueprint"],
        queryFn: () => api.get("/api/Blueprint/Player/{playerId}", { playerId }, {}),
    })

export const buyBlueprintMutation = (playerId: string, blueprintId: number, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Blueprint/{blueprintId}/Action/buy", { blueprintId }, { playerId }, {}),
        onSuccess: () => {
            queryClient.invalidateQueries({queryKey: [playerId, "blueprint"]})
            queryClient.invalidateQueries({queryKey: [playerId, "player"]})
        },
        onError
    })

export const craftBlueprintMutation = (playerId: string, blueprintId: number, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Blueprint/{blueprintId}/Action/craft", { blueprintId }, { playerId }, {}),
        onSuccess: () => {
            queryClient.invalidateQueries({queryKey: [playerId, "inventory"]})
        },
        onError
    })