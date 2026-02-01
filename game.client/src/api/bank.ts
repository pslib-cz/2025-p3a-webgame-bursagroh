import { mutationOptions, queryOptions } from "@tanstack/react-query"
import { api, queryClient } from "."

export const getBankInventoryQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "bank"],
        queryFn: () => api.getWith204("/api/Bank/{playerId}", { playerId }, {}),
    })

export const moveBankItemMutation = (playerId: string, inventoryItemId: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Bank/{playerId}/Action/move", { playerId }, {}, { inventoryItemId }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "bank"] })
            queryClient.invalidateQueries({ queryKey: [playerId, "inventory"] })
        },
    })

export const moveBankMoneyMutation = (playerId: string) =>
    mutationOptions({
        mutationFn: ({ amount, direction }: { amount: number; direction: "ToPlayer" | "ToBank" }) => api.patch("/api/Bank/{playerId}/Action/transfer", { playerId }, {}, { amount, direction }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [playerId, "player"] })
        },
    })
