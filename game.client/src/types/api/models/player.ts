import type { Floor } from "./building"

export type ScreenType = "City" | "Bank" | "Mine" | "Restaurant" | "Blacksmith" | "Floor" | "Fight"

export type Player = {
    playerId: string,
    name: string,
    money: number,
    bankBalance: number,
    screenType: ScreenType,
    positionX: number,
    positionY: number,
    subPositionX: number,
    subPositionY: number,
    floorId: number | null,
    floor: Floor | null,
    capacity: number,
    seed: number
}

export type InventoryItem = {
    inventoryItemId: number,
    playerId: string,
    itemInstanceId: number,
    isInBank: boolean,
    itemInstance: {
      itemInstanceId: number,
      itemId: number,
      durability: number,
      item: {
        itemId: number,
        name: string,
        description: string,
        itemType: number,
        weight: number,
        damage: number,
        maxDurability: number,
        changeOfGenerating: number
      }
    }
}