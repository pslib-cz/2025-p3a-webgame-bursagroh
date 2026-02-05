import type { GenericGET } from ".."
import type { Building, Floor } from "../models/building"

export interface APIGetBuildings extends GenericGET {
    query: {
        playerId: string
        top: number
        left: number
        width: number
        height: number
    }
    res: {
        200: Array<Building>
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