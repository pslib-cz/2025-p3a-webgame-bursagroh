export type Blueprint = {
    blueprintId: number
    itemId: number
    price: number
    item: {
        itemId: number
        name: string
        description: string
        itemType: "Sword"
        weight: number
        damage: number
        maxDurability: number
        changeOfGenerating: number
    }
    craftings: [
        {
            craftingId: number
            blueprintId: number
            itemId: number
            amount: number
        }
    ]
}
