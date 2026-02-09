import React from 'react'
import useNotification from './useNotification'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { useMutation } from '@tanstack/react-query'
import { mineMineBlockMutation } from '../api/mine'
import useLock from './useLock'

const useMine = () => {
    const {genericError} = useNotification()
    const handleLock = useLock()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: mineMineBlockAsync } = useMutation(mineMineBlockMutation(player.playerId, player.mineId, genericError))

    const handleMine = async (targetX: number, targetY: number) => {
        await handleLock(async () => {
            await mineMineBlockAsync({ targetX, targetY })
        })
    }

    return handleMine
}

export default useMine