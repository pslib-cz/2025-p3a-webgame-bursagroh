import React from 'react'
import type { AssetProps } from '../../../../types'
import TileSelector from '../../TileSelector'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import { validMove } from '../../../../utils/player'
import { updatePlayerPositionMutation } from '../../../../api/player'
import { useMutation } from '@tanstack/react-query'
import useNotification from '../../../../hooks/useNotification'

type FloorTileProps = {
    floorTileType: "stair" | "chest" | "floor" | "wall-top" | "wall-bottom" | "wall-left" | "wall-right" | "wall-top-left" | "wall-top-right" | "wall-bottom-left" | "wall-bottom-right" 
    | "wall-door-left-top" | "wall-door-left-right" | "wall-door-left-bottom" | "wall-door-left-left" | "wall-door-right-top" | "wall-door-right-right" | "wall-door-right-bottom" | "wall-door-right-left"
} & AssetProps

const FloorTile: React.FC<FloorTileProps> = ({ x, y, width, height, floorTileType }) => {
    const notify = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(player.playerId, x, y))

    const handleClick = () => {
        if (!validMove(player.subPositionX, player.subPositionY, x, y)) {
            notify("Error", "You cannot move that far.", 1000)
            return
        }

        updatePlayerPositionAsync()
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={floorTileType} onClick={handleClick} />
    )
}

export default FloorTile