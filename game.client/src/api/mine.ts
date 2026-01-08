import { queryOptions } from "@tanstack/react-query";
import { api } from ".";

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