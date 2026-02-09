import type { Building, BuildingType } from "../types/api/models/building"
import type { ChunkCoords, Direction } from "../types/map"

export const getChunkList = (playerPositionX: number, playerPositionY: number, horizontalViewDistanceInChunks: number, verticalViewDistanceInChunks: number, chunkSize: number): Array<ChunkCoords> => {
    const playerChunkX = Math.floor(playerPositionX / chunkSize) * chunkSize
    const playerChunkY = Math.floor(playerPositionY / chunkSize) * chunkSize
    const horizontalViewDistance = horizontalViewDistanceInChunks * chunkSize
    const verticalViewDistance = verticalViewDistanceInChunks * chunkSize

    const xFrom = playerChunkX - horizontalViewDistance
    const yFrom = playerChunkY - verticalViewDistance

    const width = horizontalViewDistanceInChunks * 2 + 1
    const height = verticalViewDistanceInChunks * 2 + 1

    return new Array(height).fill(0).flatMap((_, yIndex) => new Array(width).fill(0).map((_, xIndex) => {return {x: xFrom + xIndex * chunkSize, y: yFrom + yIndex * chunkSize}}))
}

export const getChunkCoords = (positionX: number, positionY: number, chunkSize: number): ChunkCoords => {
    return {
        x: Math.floor(positionX / chunkSize) * chunkSize,
        y: Math.floor(positionY / chunkSize) * chunkSize
    }
}

export const getTargetPosition = (direction: Direction, positionX: number, positionY: number) => {
    switch (direction) {
        case "up":
            return { x: positionX, y: positionY - 1 }
        case "down":
            return { x: positionX, y: positionY + 1 }
        case "left":
            return { x: positionX - 1, y: positionY }
        case "right":
            return { x: positionX + 1, y: positionY }
    }
}

export const mapBuildingTypeToTileType = (buildingType: BuildingType, buildingTypeTop: BuildingType | null, buildingTypeRight: BuildingType | null, buildingTypeBottom: BuildingType | null, buildingTypeLeft: BuildingType | null) => {
    switch (buildingType) {
        case "Fountain":
            return "fountain"
        case "Bank":
            return "bank"
        case "Restaurant":
            return "restaurant"
        case "Mine":
            return "mine"
        case "Blacksmith":
            return "blacksmith"
        case "Abandoned":
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-corner-bottom-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-corner-top-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-corner-top-right"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-corner-bottom-right"

            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeBottom === "Road") return "abandoned-straight-bottom"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && buildingTypeLeft === "Road") return "abandoned-straight-left"
            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeTop === "Road") return "abandoned-straight-top"
            return "abandoned-straight-right"
        case "AbandonedTrap":
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-trap-corner-bottom-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-trap-corner-top-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-trap-corner-top-right"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-trap-corner-bottom-right"

            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeBottom === "Road")
                return "abandoned-trap-straight-bottom"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && buildingTypeLeft === "Road") return "abandoned-trap-straight-left"
            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeTop === "Road") return "abandoned-trap-straight-top"
            return "abandoned-trap-straight-right"
        case "Road":
            if (buildingTypeTop === "Road" && buildingTypeRight === "Road" && buildingTypeBottom === "Road" && buildingTypeLeft === "Road") return "road"
            if (buildingTypeTop === "Road" && buildingTypeBottom === "Road") return "road-vertical"
            return "road-horizontal"
    }
}

export const buildingToChunkPosition = (
    building: Building,
    chunkSize: number
): {
    x: number
    y: number
} => {
    return {
        x: ((building.positionX % chunkSize) + chunkSize) % chunkSize,
        y: ((building.positionY % chunkSize) + chunkSize) % chunkSize,
    }
}

