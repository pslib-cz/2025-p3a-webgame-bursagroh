import type { GenericGET, GenericPATCH } from ".."
import type { Blueprint } from "../models/blueprint"

export interface APIGetBlueprints extends GenericGET {
    res: {
        200: Array<Blueprint>
    }
}

export interface APIGetPlayerBlueprints extends GenericGET {
    params: {
        playerId: string
    }
    res: {
        200: Array<Blueprint>
    }
}

export interface APIBuyBlueprint extends GenericPATCH {
    params: {
        blueprintId: number
    }
    query: {
        playerId: string
    }
    res: {
        200: object
    }
}

export interface APICraftBlueprint extends GenericPATCH {
    params: {
        blueprintId: number
    }
    query: {
        playerId: string
    }
    res: {
        200: object
    }
}