import type { GenericPOST, GenericGET, GenericPATCH } from ".."
import type { InventoryItem, Player, ScreenType } from "../models/player"

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

export interface APIPlayerInventory extends GenericGET {
    params: {
        playerId: string
    }
    res: {
        200: Array<InventoryItem>
        204: []
    }
}

export interface APIPlayerMove extends GenericPATCH {
    params: {
        playerId: string
    }
    body: {
        newPositionX: number
        newPositionY: number
        newFloorId: number | null
    }
    res: {
        200: Player
    }
}

export interface APIPlayerMoveScreen extends GenericPATCH {
    params: {
        playerId: string
    }
    body: {
        newScreenType: ScreenType
    }
    res: {
        200: Player
    }
}