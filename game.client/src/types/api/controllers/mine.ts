import type { GenericGET, GenericPATCH, GenericPOST } from ".."
import type { MineItem, MineLayer } from "../models/mine"

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

export interface APIGetMineItems extends GenericGET {
    params: {
        mineId: number
    }
    res: {
        200: Array<MineItem>
    }
}

export interface APIMineMine extends GenericPATCH {
    params: {
        mineId: number
    }
    body: {
        targetX: number
        targetY: number
    }
    res: {
        200: object
    }
}

export interface APIMineRent extends GenericPATCH {
    query: {
        playerId: string
        amount: number
    }
    res: {
        200: object
    }
}