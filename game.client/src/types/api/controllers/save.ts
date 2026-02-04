import type { GenericPOST } from ".."

export interface APISave extends GenericPOST {
    query: {
        playerId: string
    }
    res: {
        200: {
            saveString: string
        }
    }
}

export interface APILoad extends GenericPOST {
    query: {
        saveString: string
        targetPlayerId: string
    }
    res: {
        200: object
    }
}