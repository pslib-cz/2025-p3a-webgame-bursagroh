import React from "react"
import SVGDisplay from "../../components/SVGDisplay"
import Player from "../../assets/Player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useMutation, useQuery } from "@tanstack/react-query"
import { getPlayerQuery, updatePlayerScreenMutation } from "../../api/player"
import Layer from "../../components/SVG/Layer"
import TableLeft from "../../assets/tiles/TableLeft"
import TableRight from "../../assets/tiles/TableRight"
import { generateMineQuery } from "../../api/mine"
import { useNavigate } from "react-router"

const chunkSize = 16
const viewDistanceInChunks = 2

const getLayerList = (playerPositionY: number, viewDistanceInChunks: number, chunkSize: number): Array<number> => {
    const playerChunkY = playerPositionY - (playerPositionY % chunkSize)
    const viewDistance = viewDistanceInChunks * chunkSize

    const yFrom = playerChunkY - viewDistance

    const height = viewDistanceInChunks * 2

    return new Array(height).fill(0).map((_, yIndex) => yFrom + yIndex * chunkSize).filter((value) => value >= 0)
}

const MineScreen = () => {
    const navigate = useNavigate();
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = useQuery(getPlayerQuery(playerId))
    const mine = useQuery(generateMineQuery(playerId))

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    if (player.isError || mine.isError) {
        return <div>Error loading.</div>
    }

    if (player.isPending || mine.data === null) {
        return <div>Loading mine...</div>
    }

    const handleClick = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    if (player.isSuccess && mine.isSuccess) {
        const layers = getLayerList(player.data.subPositionY, viewDistanceInChunks, chunkSize)

        return (
            <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.data.subPositionX} centerY={player.data.subPositionY}>
                <TableLeft x={0} y={-1} width={1} height={1} onClick={handleClick} />
                <TableRight x={1} y={-1} width={1} height={1} onClick={handleClick} />
                {layers.map((depth) => <Layer key={`depth:${depth}`} mineId={mine.data.mine.mineId} depth={depth} size={chunkSize} />)}
                <Player x={player.data.subPositionX} y={player.data.subPositionY} width={1} height={1} />
            </SVGDisplay>
        )
    }
}

export default MineScreen
