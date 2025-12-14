import type { GenericPOST, GenericGET, GenericPATCH } from ".."
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

export interface APIPlayerMove extends GenericPATCH {
    params: {
        playerId: string
    }
    body: {
        positionX: number
        positionY: number
    }
    res: {
        200: Player
    }
}