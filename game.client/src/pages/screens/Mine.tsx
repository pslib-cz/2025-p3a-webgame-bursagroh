import React from "react"
import SVGDisplay from "../../components/SVGDisplay"
import Player from "../../assets/Player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useMutation, useQuery } from "@tanstack/react-query"
import { getPlayerQuery, updatePlayerScreenMutation } from "../../api/player"
import Layer from "../../components/SVG/Layer"
import { MineIdContext } from "../../providers/MineIdProvider"
import TableLeft from "../../assets/tiles/TableLeft"
import TableRight from "../../assets/tiles/TableRight"

const MineScreen = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = useQuery(getPlayerQuery(playerId))
    const mineId = React.useContext(MineIdContext)!.mineId

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    if (player.isError) {
        return <div>Error loading.</div>
    }

    if (player.isPending || mineId === null) {
        return <div>Loading mine...</div>
    }

    const handleClick = () => {
        updatePlayerScreenAsync()
    }

    if (player.isSuccess) {
        return (
            <SVGDisplay width={"99vw"} height={"99vh"}>
                <TableLeft x={0} y={-1} width={1} height={1} onClick={handleClick} />
                <TableRight x={1} y={-1} width={1} height={1} onClick={handleClick} />
                <Layer depth={0} />
                <Layer depth={1} />
                <Layer depth={2} />
                <Player x={player.data.floorItem.positionX} y={player.data.floorItem.positionY} width={1} height={1} />
            </SVGDisplay>
        )
    }
}

export default MineScreen
