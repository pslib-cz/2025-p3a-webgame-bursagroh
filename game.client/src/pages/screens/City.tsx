import React from "react"
import SVGDisplay from "../../components/SVGDisplay"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useQuery } from "@tanstack/react-query"
import { getPlayerQuery } from "../../api/player"
import Player from "../../assets/Player"
import Chunk from "../../components/SVG/Chunk"

type ChunkCoords = {
    x: number,
    y: number
}

const chunkSize = 16
const horizontalViewDistanceInChunks = 1
const verticalViewDistanceInChunks = 1

const getChunkList = (playerPositionX: number, playerPositionY: number, horizontalViewDistanceInChunks: number, verticalViewDistanceInChunks: number, chunkSize: number): Array<ChunkCoords> => {
    const playerChunkX = playerPositionX - (playerPositionX % chunkSize)
    const playerChunkY = playerPositionY - (playerPositionY % chunkSize)
    const horizontalViewDistance = horizontalViewDistanceInChunks * chunkSize
    const verticalViewDistance = verticalViewDistanceInChunks * chunkSize

    const xFrom = playerChunkX - horizontalViewDistance
    const yFrom = playerChunkY - verticalViewDistance

    const width = horizontalViewDistanceInChunks * 2
    const height = verticalViewDistanceInChunks * 2

    return new Array(height).fill(0).flatMap((_, yIndex) => new Array(width).fill(0).map((_, xIndex) => {return {x: xFrom + xIndex * chunkSize, y: yFrom + yIndex * chunkSize}}))
}

const CityScreen = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = useQuery(getPlayerQuery(playerId))

    if (player.isError) {
        return <div>Error loading.</div>
    }

    if (player.isPending) {
        return <div>Loading...</div>
    }

    if (player.isSuccess) {
        const chunks = getChunkList(player.data.positionX, player.data.positionY, horizontalViewDistanceInChunks, verticalViewDistanceInChunks, chunkSize)

        return (
            <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.data.positionX} centerY={player.data.positionY}>
                {chunks.map((chunk) => <Chunk key={`x:${chunk.x};y:${chunk.y}`} x={chunk.x} y={chunk.y} size={chunkSize} />)}
                <Player x={player.data.positionX} y={player.data.positionY} width={1} height={1} />
            </SVGDisplay>
        )
    }
}

export default CityScreen
