import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { ScreenType } from "../types/api/models/player"

export const generatePlayerMutation = (playerName?: string) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Player/generate", {}, {}, { name: playerName || "" }),
        onSuccess(data) {
            queryClient.setQueryData([data.playerId, "player"], data)
        },
    })

export const getPlayerQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "player"],
        queryFn: () => api.get("/api/Player/{playerId}", { playerId }, {}),
    })

export const getPlayerInventoryQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "inventory"],
        queryFn: () => api.getWith204("/api/Player/{playerId}/Inventory", { playerId }, {}),
    })

export const updatePlayerPositionMutation = (playerId: string, newPositionX: number, newPositionY: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move", { playerId }, {}, { newPositionX, newPositionY, newFloorId: null }),
        onSuccess(data) {
            queryClient.setQueryData([playerId, "player"], data)
        },
    })

export const updatePlayerFloorMutation = (playerId: string, newPositionX: number, newPositionY: number, newFloorId: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move", { playerId }, {}, { newPositionX, newPositionY, newFloorId }),
        onSuccess(data) {
            queryClient.setQueryData([playerId, "player"], data)
        },
    })

export const updatePlayerScreenMutation = (playerId: string, newScreenType: ScreenType) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move-screen", { playerId }, {}, { newScreenType }),
        onSuccess(data) {
            queryClient.setQueryData([playerId, "player"], data)
        },
    })