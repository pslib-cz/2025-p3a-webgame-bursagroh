import type { GenericGET } from ".."
import type { MineLayer } from "../models/mine"

export interface APIGenerateMine extends GenericGET {
    res: {
        200: {
            mineId: number
        }
    }
}

export interface APIGetMineLayers extends GenericGET {
    params: {
        mineId: string
        layer: string
    }
    res: {
        200: Array<MineLayer>
    }
}