import type { FloorItemInstance } from "../types/api/models/building"

export const groupFloorItems = (inventoryItems: {
    floorItemId: number;
    item: FloorItemInstance;
}[]) => {
    const itemGroupMap: Record<string, number[]> = {}

    inventoryItems.forEach((item) => {
        const key = `${item.item.item.itemId}-${item.item.durability}`
        if (itemGroupMap[key]) {
            itemGroupMap[key].push(item.floorItemId)
        } else {
            itemGroupMap[key] = [item.floorItemId]
        }
    })
    
    return itemGroupMap
}