import { mutationOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { API } from "../types/api"

export const saveMutation = (playerId: string, onSuccess: (data: API["post"]["/api/Save"]["res"][200]) => void, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Save", {}, { playerId }, {}),
        meta: { skipSaveLock: true },
        onSuccess,
        onError
    })

export const loadMutation = (targetPlayerId: string, saveString: string, onError?: (error: Error) => void) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Load", {}, { saveString, targetPlayerId }, {}),
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: [targetPlayerId], refetchType: "active" })
        },
        onError
    })