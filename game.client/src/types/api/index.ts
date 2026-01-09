import type { APIBankInventory, APIBankItemMove } from "./controllers/bank"
import type { APIGetBuildingFloor, APIGetBuildings } from "./controllers/building"
import type { APIGenerateMine, APIGetMineLayer, APIGetMineLayers, APIMineMine, APIMineRent } from "./controllers/mine"
import type { APIPlayerGenerate, APIPlayerGetById, APIPlayerInventory, APIPlayerMove, APIPlayerMoveScreen } from "./controllers/player"
import type { APIGetRandomRecipe, APIGetRecipes, APIRecipeEnd, APIRecipeStart } from "./controllers/recipe"

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
        "/api/Player/{playerId}/Inventory": APIPlayerInventory
        "/api/Bank/{playerId}": APIBankInventory
        "/api/Building/{playerId}": APIGetBuildings
        "/api/Building/{buildingId}/Interior/{level}": APIGetBuildingFloor
        "/api/Mine/{mineId}/Layer/{layer}": APIGetMineLayer
        "/api/Mine/{mineId}/Layers": APIGetMineLayers
        "/api/Recipe": APIGetRecipes
        "/api/Recipe/Random": APIGetRandomRecipe
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
        "/api/Bank/{playerId}/Action/move": APIBankItemMove
        "/api/Mine/{playerId}/Action/mine": APIMineMine
        "/api/Mine/{playerId}/Action/buy": APIMineRent
        "/api/Recipe/{recipeId}/Action/start": APIRecipeStart
        "/api/Recipe/{recipeId}/Action/end": APIRecipeEnd
    }
    delete: {

    }
}