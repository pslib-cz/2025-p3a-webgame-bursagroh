import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."

export const regenerateMineQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "mine"],
        queryFn: () => api.post("/api/Mine/Regenerate", {}, {}, { playerId }),
    })

export const getMineLayerQuery = (playerId: string, mineId: number, layer: number) =>
    queryOptions({
        queryKey: [playerId, "mine", mineId, layer],
        queryFn: () => api.get("/api/Mine/{mineId}/Layer/{layer}", { mineId, layer }, {}),
    })

export const getMineLayersQuery = (playerId: string, mineId: number, startLayer: number, endLayer: number) =>
    queryOptions({
        queryKey: [playerId, "mine", mineId, { startLayer, endLayer }],
        queryFn: () => api.get("/api/Mine/{mineId}/Layers", { mineId }, { startLayer, endLayer }),
    })

export const getMineItemsQuery = (playerId: string, mineId: number) =>
    queryOptions({
        queryKey: [playerId, "mine", mineId, "items"],
        queryFn: () => api.get("/api/Mine/{mineId}/Items", { mineId }, {}),
    })

export const mineMineBlockMutation = (playerId: string, mineId: number, targetX: number, targetY: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Mine/{mineId}/Action/mine", { mineId }, {}, { targetX, targetY }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "mine", mineId] })
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
    })

export const rentPickMutation = (playerId: string, amount: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Mine/Action/buy", {}, { playerId, amount }, {}),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
        },
    })
