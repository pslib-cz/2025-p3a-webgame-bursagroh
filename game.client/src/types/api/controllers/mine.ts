import type { GenericGET, GenericPOST } from ".."
import type { MineLayer } from "../models/mine"

export interface APIGenerateMine extends GenericPOST {
    res: {
        200: {
            mineId: number
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