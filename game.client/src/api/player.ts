import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."

export const generatePlayerMutation = (playerName?: string) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Player/generate", {}, {}, { name: playerName || "" }),
        onSuccess(data) {
            queryClient.setQueryData(["player", data.playerId], data)
        },
    })

export const getPlayerQuery = (playerId: string) =>
    queryOptions({
        queryKey: ["player", playerId],
        queryFn: () => api.get("/api/Player/{id}", { playerId }, {}),
    })
