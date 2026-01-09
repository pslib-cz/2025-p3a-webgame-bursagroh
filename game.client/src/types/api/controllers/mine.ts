import type { GenericGET, GenericPATCH, GenericPOST } from ".."
import type { MineLayer } from "../models/mine"

export interface APIGenerateMine extends GenericPOST {
    body: {
        playerId: string
    }
    res: {
        200: {
            mine: {
                mineId: number
            }
        }
    }
}

export interface APIGetMineLayer extends GenericGET {
    params: {
        mineId: number
        layer: number
    }
    res: {
        200: Array<MineLayer>
    }
}

export interface APIGetMineLayers extends GenericGET {
    params: {
        mineId: number
    }
    query: {
        startLayer: number
        endLayer: number
    }
    res: {
        200: Array<MineLayer>
    }
}

export interface APIMineMine extends GenericPATCH {
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

export interface APIMineRent extends GenericPATCH {
    params: {
        playerId: string
    }
    body: {
        amount: number
    }
    res: {
        200: object
    }
}