import React from 'react'
import Asset from '../../Asset'
import { validMove } from '../../../../utils/player'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../../../../api/player'
import useNotification from '../../../../hooks/useNotification'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import { useNavigate } from 'react-router'
import type { AssetProps } from '../../../../types'

const Minecard: React.FC<AssetProps> = ({x, y, width, height}) => {
    const { notify, genericError } = useNotification()
    const navigate = useNavigate()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(player.playerId, "City", genericError))

    const handleLeave = async () => {
        if (!validMove(player.subPositionX, player.subPositionY, x, y)) {
            notify("Error", "You cannot move that far.", 1000)
            return
        }
        
        await updatePlayerScreenAsync()
        navigate("/game/city")
    }

    return (
        <Asset assetType='minecard' x={x} y={y} width={width} height={height} onClick={handleLeave} />
    )
}

export default Minecard