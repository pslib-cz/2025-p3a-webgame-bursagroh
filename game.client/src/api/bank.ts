import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."

export const getBankInventoryQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "bank"],
        queryFn: () => api.getWith204("/api/Bank/Inventory", {}, { playerId }),
    })

export const moveBankItemMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: ({ inventoryItemIds }: { inventoryItemIds: Array<number> }) => api.patch("/api/Bank/Action/move", {}, { playerId }, { inventoryItemIds }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "bank"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
        onError
    })

export const moveBankMoneyMutation = (playerId: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: ({ amount, direction }: { amount: number; direction: "ToPlayer" | "ToBank" }) => api.patch("/api/Bank/Action/transfer", {}, { playerId }, { amount, direction }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
        },
        onError
    })
