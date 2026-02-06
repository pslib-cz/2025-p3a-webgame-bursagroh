import type { ChunkCoords } from "../types"

export const getChunkList = (playerPositionX: number, playerPositionY: number, horizontalViewDistanceInChunks: number, verticalViewDistanceInChunks: number, chunkSize: number): Array<ChunkCoords> => {
    const playerChunkX = playerPositionX - (playerPositionX % chunkSize)
    const playerChunkY = playerPositionY - (playerPositionY % chunkSize)
    const horizontalViewDistance = horizontalViewDistanceInChunks * chunkSize
    const verticalViewDistance = verticalViewDistanceInChunks * chunkSize

    const xFrom = playerChunkX - horizontalViewDistance
    const yFrom = playerChunkY - verticalViewDistance

    const width = horizontalViewDistanceInChunks * 2 + 1
    const height = verticalViewDistanceInChunks * 2 + 1

    return new Array(height).fill(0).flatMap((_, yIndex) => new Array(width).fill(0).map((_, xIndex) => {return {x: xFrom + xIndex * chunkSize, y: yFrom + yIndex * chunkSize}}))
}