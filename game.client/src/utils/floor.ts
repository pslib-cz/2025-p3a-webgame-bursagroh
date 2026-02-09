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

export const mapPositionToTileType = (x: number, y: number, buildingType: "fountain" | "bank" | "restaurant" | "mine" | "blacksmith" | "abandoned-corner-bottom-left" | "abandoned-corner-top-left" | "abandoned-corner-top-right" | "abandoned-corner-bottom-right" | "abandoned-straight-bottom" | "abandoned-straight-left" | "abandoned-straight-top" | "abandoned-straight-right" | "abandoned-trap-corner-bottom-left" | "abandoned-trap-corner-top-left" | "abandoned-trap-corner-top-right" | "abandoned-trap-corner-bottom-right" | "abandoned-trap-straight-bottom" | "abandoned-trap-straight-left" | "abandoned-trap-straight-top" | "abandoned-trap-straight-right" | "road" | "road-vertical" | "road-horizontal", isGroundFloor: boolean) => {
    if (x === 0 && y === 0) return "wall-top-left"
    if (x === 0 && y === 7) return "wall-bottom-left"
    if (x === 7 && y === 0) return "wall-top-right"
    if (x === 7 && y === 7) return "wall-bottom-right"
    if (y === 0) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-top-left" || buildingType === "abandoned-corner-top-right" || buildingType === "abandoned-straight-top" || buildingType === "abandoned-trap-corner-top-left" || buildingType === "abandoned-trap-corner-top-right" || buildingType === "abandoned-trap-straight-top") {
                if (x === 3) {
                    return "wall-door-right-top"
                } else if (x === 4) {
                    return "wall-door-left-top"
                }
            }
        }
        return "wall-top"
    }
    if (y === 7) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-bottom-left" || buildingType === "abandoned-corner-bottom-right" || buildingType === "abandoned-straight-bottom" || buildingType === "abandoned-trap-corner-bottom-left" || buildingType === "abandoned-trap-corner-bottom-right" || buildingType === "abandoned-trap-straight-bottom") {
                if (x === 3) {
                    return "wall-door-left-bottom"
                } else if (x === 4) {
                    return "wall-door-right-bottom"
                }
            }
        }
        return "wall-bottom"
    }
    if (x === 0) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-top-left" || buildingType === "abandoned-corner-bottom-left" || buildingType === "abandoned-straight-left" || buildingType === "abandoned-trap-corner-top-left" || buildingType === "abandoned-trap-corner-bottom-left" || buildingType === "abandoned-trap-straight-left") {
                if (y === 3) {
                    return "wall-door-left-left"
                } else if (y === 4) {
                    return "wall-door-right-left"
                }
            }
        }
        return "wall-left"
    }
    if (x === 7) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-top-right" || buildingType === "abandoned-corner-bottom-right" || buildingType === "abandoned-straight-right" || buildingType === "abandoned-trap-corner-top-right" || buildingType === "abandoned-trap-corner-bottom-right" || buildingType === "abandoned-trap-straight-right") {
                if (y === 3) {
                    return "wall-door-right-right"
                } else if (y === 4) {
                    return "wall-door-left-right"
                }
            }
        }
        return "wall-right"
    }
    return "floor"
}
