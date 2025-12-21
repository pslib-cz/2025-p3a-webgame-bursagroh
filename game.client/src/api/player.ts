import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { ScreenType } from "../types/api/models/player"

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
        queryFn: () => api.get("/api/Player/{playerId}", { playerId }, {}),
    })

export const updatePlayerPositionMutation = (playerId: string, newPositionX: number, newPositionY: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move", { playerId }, {}, { newPositionX, newPositionY }),
        onSuccess(data) {
            queryClient.setQueryData(["player", playerId], data)
        },
    })

export const updatePlayerScreenMutation = (playerId: string, newScreenType: ScreenType) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move-screen", { playerId }, {}, { newScreenType }),
        onSuccess(data) {
            queryClient.setQueryData(["player", playerId], data)
        },
    })