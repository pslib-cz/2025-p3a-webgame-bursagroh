import type { GenericGET, GenericPATCH } from ".."
import type { InventoryItem } from "../models/player"

export interface APIBankInventory extends GenericGET {
    params: {
        playerId: string
    }
    res: {
        200: Array<InventoryItem>
    }
}

export interface APIBankItemMove extends GenericPATCH {
    params: {
        playerId: string
    }
    body: {
        inventoryItemId: number
    }
    res: {
        200: object
    }
}