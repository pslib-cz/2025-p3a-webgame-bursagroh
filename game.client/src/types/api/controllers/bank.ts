import type { GenericGET, GenericPATCH } from ".."
import type { InventoryItem } from "../models/player"

export interface APIBankInventory extends GenericGET {
    query: {
        playerId: string
    }
    res: {
        200: Array<InventoryItem>
        204: []
    }
}

export interface APIBankItemMove extends GenericPATCH {
    query: {
        playerId: string
    }
    body: {
        inventoryItemIds: Array<number>
    }
    res: {
        200: object
    }
}

export interface APIBankMoneyTransfer extends GenericPATCH {
    query: {
        playerId: string
    }
    body: {
        amount: number
        direction: "ToPlayer" | "ToBank"
    }
    res: {
        200: object
    }
}