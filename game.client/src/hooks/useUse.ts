import React from 'react'
import { PlayerContext } from '../providers/game/PlayerProvider'
import { useItemMutation } from '../api/player'
import { useMutation } from '@tanstack/react-query'

const useUse = () => {
    const player = React.useContext(PlayerContext)!.player!

    const lock = React.useRef(false)

    const { mutateAsync: useItemAsync } = useMutation(useItemMutation(player.playerId))

    const handleUse = async () => {
        if (lock.current) return

        lock.current = true
        try {
            // eslint-disable-next-line react-hooks/rules-of-hooks
            await useItemAsync()
        } finally {
            lock.current = false
        }
    }

    return handleUse
}

export default useUse