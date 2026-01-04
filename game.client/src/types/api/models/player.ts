import type { Floor } from "./building"

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
    floorId: number | null,
    floor: Floor | null,
    capacity: number,
    seed: number
}