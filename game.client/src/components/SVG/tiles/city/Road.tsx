import React from 'react'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import { updatePlayerPositionMutation } from '../../../../api/player'
import { validMove } from '../../../../utils/player'
import { useMutation } from '@tanstack/react-query'
import TileSelector from '../../TileSelector'
import type { AssetProps } from '../../../../types'
import useNotification from '../../../../hooks/useNotification'

type RoadProps = {
    roadType: "road-vertical" | "road-horizontal" | "road"
} & AssetProps

const Road: React.FC<RoadProps> = ({ roadType, x, y, width, height }) => {
    const notify = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(player.playerId, x, y))
    
    const handleClick = () => {
        if (!validMove(player.positionX, player.positionY, x, y)) {
            notify("Error", "You cannot move that far.", 1000)
            return
        }

        updatePlayerPositionAsync()
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={roadType} onClick={handleClick} />
    )
}

export default Road