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

export const updatePlayerPositionMutation = (playerId: string, newPositionX: number, newPositionY: number, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move", { playerId }, {}, { newPositionX, newPositionY, newFloorId: null }),
        onSuccess(data) {
            queryClient.setQueryData([playerId, "player"], data)
            queryClient.invalidateQueries({ queryKey: [playerId, "floor"] })
        },
        onError
    })

export const updatePlayerFloorMutation = (playerId: string, newPositionX: number, newPositionY: number, newFloorId: number, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move", { playerId }, {}, { newPositionX, newPositionY, newFloorId }),
        onSuccess(data) {
            queryClient.setQueryData([playerId, "player"], data)
        },
        onError
    })

export const updatePlayerScreenMutation = (playerId: string, newScreenType: ScreenType, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/move-screen", { playerId }, {}, { newScreenType }),
        onSuccess(data) {
            queryClient.setQueryData([playerId, "player"], data)
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
        onError
    })

export const pickItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: (floorItemId: number) => api.patch("/api/Player/{playerId}/Action/pick", { playerId }, {}, { floorItemId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "mine"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "floor"] })
        },
        onError
    })

export const dropItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: (inventoryItemId: number) => api.patch("/api/Player/{playerId}/Action/drop", { playerId }, {}, { inventoryItemId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "mine"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "floor"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
        },
        onError
    })

export const equipItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: (inventoryItemId: number) => api.patch("/api/Player/{playerId}/Action/set-active-item", { playerId }, {}, { inventoryItemId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
        },
        onError
    })

export const useItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/use", { playerId }, {}, {}),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "floor"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
        onError
    })