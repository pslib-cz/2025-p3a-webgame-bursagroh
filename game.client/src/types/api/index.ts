import type { APIGetBuildingFloor, APIGetBuildings } from "./controllers/building"
import type { APIGenerateMine, APIGetMineLayer, APIGetMineLayers } from "./controllers/mine"
import type { APIPlayerGenerate, APIPlayerGetById, APIPlayerMove, APIPlayerMoveScreen } from "./controllers/player"

export type StringifyAble = string | number

export type Param = Record<string, StringifyAble>
export type Query = Record<string, StringifyAble>
export type PossibleResponses = Record<number, object>

export interface GenericGET {
    params: Param
    query: Query
    res: PossibleResponses
}

export interface GenericPOST {
    params: Param
    query: Query
    body: object
    res: PossibleResponses
}

export interface GenericPUT {
    params: Param
    query: Query
    body: object
    res: PossibleResponses
}

export interface GenericPATCH {
    params: Param
    query: Query
    body: object
    res: PossibleResponses
}

export interface GenericDELETE {
    params: Param
    query: Query
    res: PossibleResponses
}

export interface GenericAPI {
    get: {
        [key: string]: GenericGET
    }
    post: {
        [key: string]: GenericPOST
    }
    put: {
        [key: string]: GenericPUT
    }
    patch: {
        [key: string]: GenericPATCH
    }
    delete: {
        [key: string]: GenericDELETE
    }
}

export interface API extends GenericAPI {
    get: {
        "/api/Player/{playerId}": APIPlayerGetById
        "/api/Building/{playerId}": APIGetBuildings
        "/api/Building/{buildingId}/Interior/{level}": APIGetBuildingFloor
        "/api/Mine/{mineId}/Layer/{layer}": APIGetMineLayer
        "/api/Mine/{mineId}/Layers": APIGetMineLayers
    }
    post: {
        "/api/Player/generate": APIPlayerGenerate
        "/api/Mine/Generate": APIGenerateMine
    }
    put: {

    }
    patch: {
        "/api/Player/{playerId}/Action/move": APIPlayerMove
        "/api/Player/{playerId}/Action/move-screen": APIPlayerMoveScreen
    }
    delete: {

    }
}