import { mutationOptions, queryOptions } from "@tanstack/react-query";
import { api, queryClient } from ".";

export const generateMineQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "mine"],
        queryFn: () => api.post("/api/Mine/Generate", {}, {}, { playerId }),
    })

export const getMineLayerQuery = (playerId: string, mineId: number, layer: number) =>
    queryOptions({
        queryKey: [playerId, "mine", mineId, layer],
        queryFn: () => api.get("/api/Mine/{mineId}/Layer/{layer}", {mineId, layer}, {}),
    })

export const getMineLayersQuery = (playerId: string, mineId: number, startLayer: number, endLayer: number) =>
    queryOptions({
        queryKey: [playerId, "mine", mineId, {startLayer, endLayer}],
        queryFn: () => api.get("/api/Mine/{mineId}/Layers", {mineId}, {startLayer, endLayer}),
    })

export const mineMineBlockMutation = (playerId: string, mineId: number, inventoryItemId: number, targetX: number, targetY: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Mine/{playerId}/Action/mine", { playerId }, {}, { inventoryItemId, targetX, targetY }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "mine", mineId] })
        },
    })

export const rentPickMutation = (playerId: string, amount: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Mine/{playerId}/Action/buy", { playerId }, {}, { amount }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
    })