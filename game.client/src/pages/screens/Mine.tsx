import React from "react"
import SVGDisplay from "../../components/SVGDisplay"
import Player from "../../assets/Player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useMutation, useQuery } from "@tanstack/react-query"
import { getPlayerQuery, updatePlayerScreenMutation } from "../../api/player"
import Layer from "../../components/SVG/Layer"
import TableLeft from "../../assets/tiles/TableLeft"
import TableRight from "../../assets/tiles/TableRight"
import { generateMineQuery, rentPickMutation } from "../../api/mine"
import { useNavigate } from "react-router"
import Tile from "../../components/SVG/Tile"

const chunkSize = 16
const viewDistanceInChunks = 2

const getLayerList = (playerPositionY: number, viewDistanceInChunks: number, chunkSize: number): Array<number> => {
    const playerChunkY = playerPositionY - (playerPositionY % chunkSize)
    const viewDistance = viewDistanceInChunks * chunkSize

    const yFrom = playerChunkY - viewDistance

    const height = viewDistanceInChunks * 2

    return new Array(height)
        .fill(0)
        .map((_, yIndex) => yFrom + yIndex * chunkSize)
        .filter((value) => value >= 0)
}

const MineScreen = () => {
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = useQuery(getPlayerQuery(playerId))
    const mine = useQuery(generateMineQuery(playerId))

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))
    const { mutateAsync: rentPickAsync } = useMutation(rentPickMutation(playerId, 1))

    if (player.isError || mine.isError) {
        return <div>Error loading.</div>
    }

    if (player.isPending || mine.data === null) {
        return <div>Loading mine...</div>
    }

    const handleLeave = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    const handleBuy = async () => {
        await rentPickAsync()
    }

    if (player.isSuccess && mine.isSuccess) {
        const layers = getLayerList(player.data.subPositionY, viewDistanceInChunks, chunkSize)

        return (
            <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.data.subPositionX} centerY={player.data.subPositionY}>
                <TableLeft x={1} y={-3} width={1} height={1} onClick={handleLeave} />
                <TableRight x={2} y={-3} width={1} height={1} onClick={handleBuy} />

                <Tile x={0} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={1} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={2} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={3} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={4} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={5} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={6} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={7} y={-1} width={1} height={1} tileType="empty" />

                <Tile x={1} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={2} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={3} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={4} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={5} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={6} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={7} y={-2} width={1} height={1} tileType="empty" />

                {layers.map((depth) => (
                    <Layer key={`depth:${depth}`} mineId={mine.data.mine.mineId} depth={depth} size={chunkSize} />
                ))}
                
                <Player x={player.data.subPositionX} y={player.data.subPositionY} width={1} height={1} />
            </SVGDisplay>
        )
    }
}

export default MineScreen
