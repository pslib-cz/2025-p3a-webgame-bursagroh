import type { ChunkCoords } from "../types"

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