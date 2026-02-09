import React from 'react'
import useNotification from './useNotification'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerPositionMutation } from '../api/player'
import useLock from './useLock'

const useMove = () => {
    const { genericError } = useNotification()
    const handleLock = useLock()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(player.playerId, genericError))

    const handleMove = async (newPositionX: number, newPositionY: number) => {
        await handleLock(async () => {
            await updatePlayerPositionAsync({ newPositionX, newPositionY })
        })
    }

    return handleMove
}

export default useMove