import { queryOptions } from "@tanstack/react-query";
import { api } from ".";

export const getPlayerQuery = (playerId: string) =>
    queryOptions({
        queryKey: ["player", playerId],
        queryFn: () => api.get("/api/Player/{id}", { playerId }, {}),
    })
