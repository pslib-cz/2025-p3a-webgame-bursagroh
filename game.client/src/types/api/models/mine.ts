export type BlockType = "Wooden_Frame" | "Rock" | "Copper_Ore" | "Iron_Ore" | "Gold_Ore" | "Silver_Ore" | "Unobtanium_Ore"

export type MineLayer = {
    mineLayerID: number
    mineId: number
    depth: number
    mineBlocks: Array<{
        mineBlockId: number
        mineLayerId: number
        index: number
        blockId: number
        block: {
            blockId: number
            blockType: BlockType
            itemId: number
            item: {
                itemId: number
                name: string
                description: string | null
                itemType: number
                weight: number
                damage: number
                maxDurability: number
                changeOfGenerating: number
            }
            minAmount: number
            maxAmount: number
        }
        health: number
    }>
}

export type MineItem = {
    floorItemId: number,
    floorId: number,
    positionX: number,
    positionY: number,
    itemInstanceId: number,
    itemInstance: {
      itemInstanceId: number,
      itemId: number,
      durability: number,
      item: {
        itemId: number,
        name: string,
        description: string,
        itemType: string,
        weight: number,
        damage: number,
        maxDurability: number,
        changeOfGenerating: number
      }
    },
}