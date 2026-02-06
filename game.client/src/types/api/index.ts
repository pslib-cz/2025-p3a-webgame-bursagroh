import type { APIBankInventory, APIBankItemMove, APIBankMoneyTransfer } from "./controllers/bank"
import type { APIBuyBlueprint, APICraftBlueprint, APIGetBlueprints, APIGetPlayerBlueprints } from "./controllers/blueprint"
import type { APIGetBuildings, APIGetFloor } from "./controllers/building"
import type { APIGenerateMine, APIGetMineItems, APIGetMineLayer, APIGetMineLayers, APIMineMine, APIMineRent } from "./controllers/mine"
import type { APIPlayerGenerate, APIPlayerGetById, APIPlayerInventory, APIPlayerItemDrop, APIPlayerItemEquip, APIPlayerItemPick, APIPlayerMove, APIPlayerMoveScreen, APIPlayerUse } from "./controllers/player"
import type { APIGetLeaderboard, APIGetRandomRecipe, APIGetRecipes, APIRecipeEnd, APIRecipeStart } from "./controllers/recipe"
import type { APILoad, APISave } from "./controllers/save"

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
        "/api/Bank/Inventory": APIBankInventory
        "/api/Building": APIGetBuildings
        "/api/Building/Floor/{floorId}": APIGetFloor
        "/api/Mine/{mineId}/Layer/{layer}": APIGetMineLayer
        "/api/Mine/{mineId}/Layers": APIGetMineLayers
        "/api/Mine/{mineId}/Items": APIGetMineItems
        "/api/Recipe": APIGetRecipes
        "/api/Recipe/Random": APIGetRandomRecipe
        "/api/Recipe/Leaderboard": APIGetLeaderboard
        "/api/Blueprint": APIGetBlueprints
        "/api/Blueprint/Player/{playerId}": APIGetPlayerBlueprints
    }
    post: {
        "/api/Player/Generate": APIPlayerGenerate
        "/api/Mine/Regenerate": APIGenerateMine
        "/api/Save": APISave
        "/api/Load": APILoad
    }
    put: {

    }
    patch: {
        "/api/Player/{playerId}/Action/move": APIPlayerMove
        "/api/Player/{playerId}/Action/move-screen": APIPlayerMoveScreen
        "/api/Player/{playerId}/Action/pick": APIPlayerItemPick
        "/api/Player/{playerId}/Action/drop": APIPlayerItemDrop
        "/api/Bank/Action/move": APIBankItemMove
        "/api/Bank/Action/transfer": APIBankMoneyTransfer
        "/api/Mine/{mineId}/Action/mine": APIMineMine
        "/api/Mine/Action/buy": APIMineRent
        "/api/Recipe/{recipeId}/Action/start": APIRecipeStart
        "/api/Recipe/{recipeId}/Action/end": APIRecipeEnd
        "/api/Blueprint/{blueprintId}/Action/buy": APIBuyBlueprint
        "/api/Blueprint/{blueprintId}/Action/craft": APICraftBlueprint
        "/api/Player/{playerId}/Action/set-active-item": APIPlayerItemEquip
        "/api/Player/{playerId}/Action/use": APIPlayerUse
    }
    delete: {

    }
}

export type NoContentURL = "/api/Player/{playerId}/Inventory" | "/api/Bank/Inventory"