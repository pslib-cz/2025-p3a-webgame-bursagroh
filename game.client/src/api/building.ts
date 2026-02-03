import { queryOptions } from "@tanstack/react-query"
import { api } from "."

export const getBuildingsQuery = (playerId: string, top: number, left: number, width: number, height: number) =>
    queryOptions({
        queryKey: [playerId, "buildings", { top, left, width, height }],
        queryFn: () => api.get("/api/Building", {}, { playerId, top, left, width, height }),
    })

export const getFloorQuery = (playerId: string, floorId: number) =>
    queryOptions({
        queryKey: [playerId, "floor", floorId],
        queryFn: () => api.get("/api/Building/Floor/{floorId}", { floorId }, {}),
    })
