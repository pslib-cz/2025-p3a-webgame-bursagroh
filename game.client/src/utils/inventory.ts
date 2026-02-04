import type { InventoryItem } from "../types/api/models/player";

export const calcInventoryWeight = (inventoryItems: Array<InventoryItem>) => {
    return inventoryItems.reduce((totalWeight, item) => {
        return totalWeight + item.itemInstance.item.weight
    }, 0)
}

export const groupInventoryItems = (inventoryItems: Array<InventoryItem>) => {
    const itemGroupMap: Record<string, number[]> = {}

    inventoryItems.forEach((item) => {
        const key = `${item.itemInstance.item.itemId}-${item.itemInstance.durability}`
        if (itemGroupMap[key]) {
            itemGroupMap[key].push(item.inventoryItemId)
        } else {
            itemGroupMap[key] = [item.inventoryItemId]
        }
    })
    
    return itemGroupMap
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