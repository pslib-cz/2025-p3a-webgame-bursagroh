import type { GenericGET, GenericPATCH } from ".."
import type { Building, Floor } from "../models/building"

export interface APIGetBuildings extends GenericGET {
    params: {
        playerId: string
    }
    query: {
        top: number
        left: number
        width: number
        height: number
    }
    res: {
        200: Array<Building>
    }
}

export interface APIGetBuildingFloor extends GenericGET {
    params: {
        buildingId: number
        level: number
    }
    query: {
        playerId: string
    }
    res: {
        200: Floor
    }
}

export interface APIGetFloor extends GenericGET {
    params: {
        floorId: number
    }
    res: {
        200: Floor
    }
}

export interface APIInteractInBuilding extends GenericPATCH {
    params: {
        playerId: string
    }
    body: {
        inventoryItemId: number
        targetX: number
        targetY: number
    }
    res: {
        200: object
    }
}