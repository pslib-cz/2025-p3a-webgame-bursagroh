import { queryOptions } from "@tanstack/react-query";
import { api } from ".";

export const getBuildingsQuery = (playerId: string, top: number, left: number, width: number, height: number) =>
    queryOptions({
        queryKey: [playerId, "buildings", {top, left, width, height}],
        queryFn: () => api.get("/api/Building/{playerId}", { playerId }, {top, left, width, height}),
    })

export const getBuildingFloorQuery = (playerId: string, buildingId: number, level: number) =>
    queryOptions({
        queryKey: [playerId, "building", buildingId, level],
        queryFn: () => api.get("/api/Building/{buildingId}/Interior/{level}", {buildingId, level}, {playerId}),
    })
