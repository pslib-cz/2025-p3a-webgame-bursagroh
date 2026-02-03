export type ScreenType = "City" | "Bank" | "Mine" | "Restaurant" | "Blacksmith" | "Floor" | "Fight" | "Fountain" | "Win"

export type Player = {
    playerId: string
    name: string
    money: number
    bankBalance: number
    screenType: ScreenType
    positionX: number
    positionY: number
    subPositionX: number
    subPositionY: number
    floorId: number | null
    mineId: number
    activeInventoryItemId: number | null
    capacity: number
    health: number
}

export type InventoryItem = {
    inventoryItemId: number
    itemInstance: {
        itemInstanceId: number
        durability: number
        item: {
            itemId: number
            name: string
            description: string
            itemType: string
            weight: number
            damage: number
            maxDurability: number
        }
    }
}
