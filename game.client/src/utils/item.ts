import type { AssetType } from "../types/asset";

export const itemIdToAssetType = (itemId: number): AssetType => {
    switch (itemId) {
        case 1:
            return "wooden_frame"
        case 2:
            return "rock_item"
        case 3:
            return "copper"
        case 4:
            return "iron"
        case 5:
            return "silver"
        case 6:
            return "gold"
        case 7:
            return "unobtainium"
        case 10:
            return "wooden_sword"
        case 30:
            return "wooden_pickaxe"
        default:
            return "empty"
    }
}