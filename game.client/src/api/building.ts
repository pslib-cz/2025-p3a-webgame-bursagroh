import { queryOptions } from "@tanstack/react-query";
import { api } from ".";

export const getBuildingsQuery = (playerId: string) =>
    queryOptions({
        queryKey: ["player", playerId, "buildings"],
        queryFn: () => api.get("/api/Building/{playerId}", { playerId }, {}),
    })
