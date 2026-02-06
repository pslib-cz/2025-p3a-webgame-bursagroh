import type { AssetType } from "../types/asset"

export const itemIdToAssetType = (itemId: number): AssetType | undefined => {
    switch (itemId) {
        case 1:
            return "wood"
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
        case 11:
            return "rock_sword"
        case 12:
            return "copper_sword"
        case 13:
            return "iron_sword"
        case 14:
            return "silver_sword"
        case 15:
            return "gold_sword"
        case 16:
            return "unobtainium_sword"

        case 20:
            return "wooden_axe"
        case 21:
            return "rock_axe"
        case 22:
            return "copper_axe"
        case 23:
            return "iron_axe"
        case 24:
            return "silver_axe"
        case 25:
            return "gold_axe"
        case 26:
            return "unobtainium_axe"

        case 30:
            return "wooden_pickaxe"
        case 31:
            return "rock_pickaxe"
        case 32:
            return "copper_pickaxe"
        case 33:
            return "iron_pickaxe"
        case 34:
            return "silver_pickaxe"
        case 35:
            return "gold_pickaxe"
        case 36:
            return "unobtainium_pickaxe"

        case 39:
            return "rented_pickaxe"

        case 40:
            return "health_potion"
        case 41:
            return "endurance_potion"
        case 42:
            return "strength_potion"

        case 100:
            return "mythical_sword"
    }
}
