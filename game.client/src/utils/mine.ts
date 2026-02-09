import { NEXT_TO_TABLE_POSITIONS } from "../constants/mine"
import type { BlockType } from "../types/api/models/mine"
import type { Player } from "../types/api/models/player"

export const getLayerList = (playerPositionY: number, viewDistanceInChunks: number, chunkSize: number): Array<number> => {
    const playerChunkY = playerPositionY - (playerPositionY % chunkSize)
    const viewDistance = viewDistanceInChunks * chunkSize

    const yFrom = playerChunkY - viewDistance

    const height = viewDistanceInChunks * 2 + 1

    return new Array(height)
        .fill(0)
        .map((_, yIndex) => yFrom + yIndex * chunkSize)
        .filter((value) => value >= 0)
}

export const isPlayerNextToTable = (player: Player) => {
    return NEXT_TO_TABLE_POSITIONS.some(pos => pos.x === player.subPositionX && pos.y === player.subPositionY)
}

export const mapBlockTypeToTileType = (buildingType: BlockType) => {
    switch (buildingType) {
        case "Wooden_Frame":
            return "wooden_frame"
        case "Rock":
            return "rock"
        case "Copper_Ore":
            return "copper_ore"
        case "Iron_Ore":
            return "iron_ore"
        case "Gold_Ore":
            return "gold_ore"
        case "Silver_Ore":
            return "silver_ore"
        case "Unobtanium_Ore":
            return "unobtainium_ore"
    }
}

