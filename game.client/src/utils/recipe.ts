import type { IngredienceType } from "../types/api/models/recipe";
import type { AssetType } from "../types/asset";

export const reassembleIngrediences = (ingrediences: Array<IngredienceType>) => {
    return ingrediences.map((ingredience) => ({ type: ingredience }))
}

export const ingredienceToAssetType = (ingredienceType: IngredienceType): AssetType => {
    switch (ingredienceType) {
        case 'Meat':
            return "meat"
        case 'Salad':
            return "salad"
        case 'BunUp':
            return "bun_up"
        case 'BunDown':
            return "bun_down"
        case 'Tomato':
            return "tomato"
        case 'Sauce':
            return "sauce"
        case 'Bacon':
            return "bacon"
        case 'Cheese':
            return "cheese"
    }
}
