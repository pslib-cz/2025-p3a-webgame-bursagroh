import type { GenericGET } from ".."
import type { Building } from "../models/building"

export interface APIGetBuildings extends GenericGET {
    params: {
        playerId: string
    }
    res: {
        200: Array<Building>
    }
}
