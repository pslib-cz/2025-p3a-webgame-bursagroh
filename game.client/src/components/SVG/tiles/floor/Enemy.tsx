import React from 'react'
import TileSelector from '../../TileSelector'
import type { AssetProps, EnemyType } from '../../../../types'
import { useNavigate } from 'react-router'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerPositionMutation } from '../../../../api/player'
import { validMove } from '../../../../utils/player'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import useNotification from '../../../../hooks/useNotification'

type EnemyProps = {
    enemyType: EnemyType
} & AssetProps

const Enemy: React.FC<EnemyProps> = ({ width, height, x, y, enemyType }) => {
    const navigate = useNavigate()
    const notify = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(player.playerId, x, y))

    const handleClick = () => {
        if (!validMove(player.subPositionX, player.subPositionY, x, y)) {
            notify("Error", "You cannot move that far.", 1000)
            return
        }

        updatePlayerPositionAsync()
        navigate("/game/fight")
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={enemyType} onClick={handleClick} />
    )
}

export default Enemy