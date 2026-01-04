import { queryOptions } from "@tanstack/react-query";
import { api } from ".";

export const generateMineQuery = () =>
    queryOptions({
        queryKey: ["mine"],
        queryFn: () => api.post("/api/Mine/Generate", {}, {}, {}),
    })

export const getMineLayerQuery = (mineId: number, layer: number) =>
    queryOptions({
        queryKey: ["mine", mineId, layer],
        queryFn: () => api.get("/api/Mine/{mineId}/Layer/{layer}", {mineId, layer}, {}),
    })

export const getMineLayersQuery = (mineId: number, startLayer: number, endLayer: number) =>
    queryOptions({
        queryKey: ["mine", mineId, {startLayer, endLayer}],
        queryFn: () => api.get("/api/Mine/{mineId}/Layers", {mineId}, {startLayer, endLayer}),
    })