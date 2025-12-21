export type ScreenType = "City" | "Bank" | "Mine" | "Restaurant" | "Blacksmith" | "Floor" | "Fight"

export type Player = {
    playerId: string,
    name: string,
    money: number,
    screenType: ScreenType,
    buildingId: null,
    floorItemId: null,
    floorItem: {
        floorItemId: 1,
        floorId: 1,
        positionX: 1,
        positionY: 1,
        floorItemType: "Stair"
    },
    capacity: number,
    seed: number
}