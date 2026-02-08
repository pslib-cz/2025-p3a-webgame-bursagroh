import React from 'react'
import useNotification from './useNotification'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerPositionMutation } from '../api/player'
import { validMove } from '../utils/player'

const useMove = () => {
    const { notify, genericError } = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const lock = React.useRef(false)

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(player.playerId, genericError))

    const handleMove = async (newPositionX: number, newPositionY: number, isSubMove: boolean) => {
        const playerX = isSubMove ? player.subPositionX : player.positionX
        const playerY = isSubMove ? player.subPositionY : player.positionY

        if (!validMove(playerX, playerY, newPositionX, newPositionY)) {
            notify("Error", "You cannot move that far.", 1000)
            return
        }

        if (lock.current) return

        lock.current = true
        try {
            await updatePlayerPositionAsync({ newPositionX, newPositionY })
        } finally {
            lock.current = false
        }
    }

    return handleMove
}

export default useMove