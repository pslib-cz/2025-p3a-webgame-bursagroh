export type Blueprint = {
    blueprintId: number
    price: number
    item: {
        itemId: number
        name: string
        description: string
        itemType: "Sword"
        weight: number
        damage: number
        maxDurability: number
    }
    craftings: [
        {
            item: {
                itemId: number
                name: string
            }
            amount: number
        },
    ]
}
