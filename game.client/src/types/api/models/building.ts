export type BuildingType = "Fountain" | "Bank" | "Restaurant" | "Mine" | "Blacksmith" | "Abandoned" | "AbandonedTrap"

export type Building = {
    buildingId: number
    playerId: string
    positionX: number
    positionY: number
} & ({
    buildingType: "Fountain" | "Bank" | "Restaurant" | "Mine" | "Blacksmith"
} | {
    buildingType: "Abandoned" | "AbandonedTrap"
    height: number
    reachedHeight: number
    isBossDefeated: boolean
    // floors: [
    //     {
    //         floorId: 1
    //         buildingId: 1
    //         level: 1
    //         floorItems: [
    //             {
    //                 floorItemId: 1
    //                 floorId: 1
    //                 positionX: 1
    //                 positionY: 1
    //                 floorItemType: "Stair"
    //             }
    //         ]
    //     }
    // ]
})