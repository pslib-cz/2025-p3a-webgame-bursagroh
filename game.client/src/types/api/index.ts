import type { APIPlayerGenerate, APIPlayerGetById } from "./controllers/player"

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
        "/api/Player/{id}": APIPlayerGetById
    }
    post: {
        "/api/Player/generate": APIPlayerGenerate
        "idk": APIPlayerGenerate
    }
    put: {

    }
    patch: {

    }
    delete: {

    }
}