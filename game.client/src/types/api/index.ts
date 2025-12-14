import type { APIGetBuildings } from "./controllers/building"
import type { APIPlayerGenerate, APIPlayerGetById, APIPlayerMove } from "./controllers/player"

export interface GenericGET {
    params: Record<string, string>
    query: Record<string, string>
    res: Record<number, object>
}

export interface GenericPOST {
    params: Record<string, string>
    query: Record<string, string>
    body: object
    res: Record<number, object>
}

export interface GenericPUT {
    params: Record<string, string>
    query: Record<string, string>
    body: object
    res: Record<number, object>
}

export interface GenericPATCH {
    params: Record<string, string>
    query: Record<string, string>
    body: object
    res: Record<number, object>
}

export interface GenericDELETE {
    params: Record<string, string>
    query: Record<string, string>
    res: Record<number, object>
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
    }
    post: {
        "/api/Player/generate": APIPlayerGenerate
    }
    put: {

    }
    patch: {
        "/api/Player/{playerId}/Action/move": APIPlayerMove
    }
    delete: {

    }
}