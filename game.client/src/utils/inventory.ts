import type { InventoryItem } from "../types/api/models/player";

export const calcInventoryWeight = (inventoryItems: Array<InventoryItem>) => {
    return inventoryItems.reduce((totalWeight, item) => {
        return totalWeight + item.itemInstance.item.weight
    }, 0)
}

export const countInventoryItems = (inventoryItems: Array<InventoryItem>) => {
    const itemCountMap: Record<number, number> = {}

    inventoryItems.forEach((item) => {
        const itemId = item.itemInstance.item.itemId
        if (itemCountMap[itemId]) {
            itemCountMap[itemId] += 1
        } else {
            itemCountMap[itemId] = 1
        }
    })
    
    return itemCountMap
}

export const removeEquippedItemFromInventory = (inventoryItems: Array<InventoryItem>, equippedInventoryItemId: number | null) => {
    if (equippedInventoryItemId === null) {
        return inventoryItems
    }

    const index = inventoryItems.findIndex(item => item.inventoryItemId === equippedInventoryItemId)
    
    if (index !== -1) {
        inventoryItems.splice(index, 1)
    }
    
    return inventoryItems
}