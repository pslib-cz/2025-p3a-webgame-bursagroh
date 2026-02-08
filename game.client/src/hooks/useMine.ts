import React from 'react'
import useNotification from './useNotification'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { useMutation } from '@tanstack/react-query'
import { mineMineBlockMutation } from '../api/mine'
import { validMove } from '../utils/player'

const useMine = () => {
    const {notify, genericError} = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const lock = React.useRef(false)

    const { mutateAsync: mineMineBlockAsync } = useMutation(mineMineBlockMutation(player.playerId, player.mineId, genericError))

    const handleMine = async (targetX: number, targetY: number) => {
        if (!validMove(player.subPositionX, player.subPositionY, targetX, targetY)) {
            notify("Error", "You cannot mine that far.", 1000)
            return
        }

        if (lock.current) return

        lock.current = true
        try {
            await mineMineBlockAsync({ targetX, targetY })
        } finally {
            lock.current = false
        }
    }

    return handleMine
}

export default useMine