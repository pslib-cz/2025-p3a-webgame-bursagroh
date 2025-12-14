import { queryOptions } from "@tanstack/react-query";
import { api } from ".";

export const generateMineQuery = () =>
    queryOptions({
        queryKey: ["mine"],
        queryFn: () => api.get("/api/Mine/Generate", {}, {}),
    })

export const getMineLayerQuery = (mineId: number, layer: number) =>
    queryOptions({
        queryKey: ["mine", mineId, "layer", layer],
        queryFn: () => api.get("/api/Mine/{mineId}/Layer/{layer}", { mineId: mineId.toString(), layer: layer.toString() }, {}),
    })