import { mutationOptions, queryOptions } from "@tanstack/react-query";
import { api, queryClient } from ".";

export const getBankInventoryQuery = (playerId: string) =>
    queryOptions({
        queryKey: [playerId, "bank"],
        queryFn: () => api.get("/api/Bank/{playerId}", { playerId }, {}),
    })

export const moveBankItemMutation = (playerId: string, inventoryItemId: number) =>
    mutationOptions({
        mutationFn: () => api.patch("/api/Bank/{playerId}/Action/move", { playerId }, {}, {inventoryItemId}),
        onSuccess: () => {
            queryClient.invalidateQueries({queryKey: [[playerId, "bank"], [playerId, "inventory"]]})
        },
    })