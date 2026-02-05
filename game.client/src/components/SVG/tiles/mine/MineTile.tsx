import React from 'react'
import type { AssetProps } from '../../../../types'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import { updatePlayerPositionMutation } from '../../../../api/player'
import { validMove } from '../../../../utils/player'
import TileSelector from '../../TileSelector'
import { useMutation } from '@tanstack/react-query'

type MineTileProps = {
    mineTileType: "empty"
} & AssetProps

const MineTile: React.FC<MineTileProps> = ({x, y, width, height, mineTileType}) => {
    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(player.playerId, x, y))
    
    const handleClick = () => {
        if (!validMove(player.subPositionX, player.subPositionY, x, y)) return

        updatePlayerPositionAsync()
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={mineTileType} onClick={handleClick} />
    )
}

export default MineTile