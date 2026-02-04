import { mutationOptions } from "@tanstack/react-query"
import { api, queryClient } from "."
import type { API } from "../types/api"

export const saveMutation = (playerId: string, onSuccess: (data: API["post"]["/api/Save"]["res"][200]) => void) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Save", {}, { playerId }, {}),
        onSuccess
    })

export const loadMutation = (targetPlayerId: string, saveString: string) =>
    mutationOptions({
        mutationFn: () => api.post("/api/Load", {}, { saveString, targetPlayerId }, {}),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [targetPlayerId] })
        }
    })