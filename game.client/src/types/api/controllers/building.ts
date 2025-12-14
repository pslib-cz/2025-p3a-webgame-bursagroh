import type { GenericGET } from ".."

export interface APIPlayerGetById extends GenericGET {
    params: {
        id: string
    }
    res: {
        200: Player
    }
}
