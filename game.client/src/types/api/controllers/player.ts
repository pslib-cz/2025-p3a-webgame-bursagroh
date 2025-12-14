import type { GenericPOST, GenericGET } from ".."
import type { Player } from "../models/player"

export interface APIPlayerGenerate extends GenericPOST {
    body: {
        name: string
    }
    res: {
        200: Player
    }
}

export interface APIPlayerGetById extends GenericGET {
    params: {
        playerId: string
    }
    res: {
        200: Player
    }
}
