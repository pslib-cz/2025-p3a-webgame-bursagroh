import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { ScreenType } from "../types/api/models/player"

export const generatePlayerMutation = (playerName?: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Player/Generate", {}, {}, { name: playerName || "" }),
        onSuccess(data) {
            queryClient.setQueryData([data.playerId, "player"], data)
        },
        onError
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

export const updatePlayerPositionMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: ({ newPositionX, newPositionY }: { newPositionX: number, newPositionY: number }) => api.patch("/api/Player/{playerId}/Action/move", { playerId }, {}, { newPositionX, newPositionY, newFloorId: null }),
        onSuccess: async (data) => {
            queryClient.setQueryData([playerId, "player"], data)
            await queryClient.invalidateQueries({ queryKey: [playerId, "floor"], refetchType: "active" })
        },
        onError
    })

export const updatePlayerScreenMutation = (playerId: string, newScreenType: ScreenType, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move-screen", { playerId }, {}, { newScreenType }),
        onSuccess: async (data) => {
            queryClient.setQueryData([playerId, "player"], data)
            await queryClient.invalidateQueries({ queryKey: [playerId, "inventory"], refetchType: "active" })
        },
        onError
    })

export const pickItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: (floorItemId: number) => api.patch("/api/Player/{playerId}/Action/pick", { playerId }, {}, { floorItemId }),
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: [playerId, "inventory"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: [playerId, "mine"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: [playerId, "floor"], refetchType: "active" })
            ])
        },
        onError
    })

export const dropItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: (inventoryItemId: number) => api.patch("/api/Player/{playerId}/Action/drop", { playerId }, {}, { inventoryItemId }),
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: [playerId, "inventory"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: [playerId, "mine"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: [playerId, "floor"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: [playerId, "player"], refetchType: "active" })
            ])
        },
        onError
    })

export const equipItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: (inventoryItemId: number) => api.patch("/api/Player/{playerId}/Action/set-active-item", { playerId }, {}, { inventoryItemId }),
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: [playerId, "player"], refetchType: "active" })
        },
        onError
    })

export const useItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/use", { playerId }, {}, {}),
        onSuccess: async () => {
            await Promise.all([
                queryClient.invalidateQueries({ queryKey: [playerId, "player"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: [playerId, "floor"], refetchType: "active" }),
                queryClient.invalidateQueries({ queryKey: [playerId, "inventory"], refetchType: "active" })
            ])
        },
        onError
    })