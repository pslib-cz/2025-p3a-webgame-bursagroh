export type ScreenType = "City" | "Bank" | "Mine" | "Restaurant" | "Blacksmith" | "Floor" | "Fight"

export type Player = {
    playerId: string,
    name: string,
    money: number,
    screenType: ScreenType,
    positionX: number,
    positionY: number,
    subPositionX: number,
    subPositionY: number,
    floorId: null,
    capacity: number,
    seed: number
}