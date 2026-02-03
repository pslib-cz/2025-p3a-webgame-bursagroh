import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { ScreenType } from "../types/api/models/player"

export const generatePlayerMutation = (playerName?: string) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Player/Generate", {}, {}, { name: playerName || "" }),
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
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
    })

export const pickItemMutation = (playerId: string, mineId: number, buildingId: number, level: number) =>
    mutationOptions({
        mutationFn: (floorItemId: number) => api.patch("/api/Player/{playerId}/Action/pick", { playerId }, {}, { floorItemId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "mine", mineId] })
            queryClient.invalidateQueries({queryKey: [playerId, "building", buildingId, level]})
        },
    })

export const dropItemMutation = (playerId: string) =>
    mutationOptions({
        mutationFn: (inventoryItemId: number) => api.patch("/api/Player/{playerId}/Action/drop", { playerId }, {}, { inventoryItemId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
    })

export const equipItemMutation = (playerId: string) =>
    mutationOptions({
        mutationFn: (inventoryItemId: number) => api.patch("/api/Player/{playerId}/Action/set-active-item", { playerId }, {}, { inventoryItemId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
        },
    })

export const useItemMutation = (playerId: string, floorId: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Player/{playerId}/Action/use", { playerId }, {}, {}),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "floor", floorId] })
        },
    })