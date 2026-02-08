import React from 'react'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { useItemMutation } from '../api/player'
import { useMutation } from '@tanstack/react-query'
import useNotification from './useNotification'
import useLock from './useLock'

const useUse = () => {
    const {genericError} = useNotification()
    const handleLock = useLock()
    
    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: useItemAsync } = useMutation(useItemMutation(player.playerId, genericError))

    const handleUse = async () => {
        await handleLock(useItemAsync)
    }

    return handleUse
}

export default useUse