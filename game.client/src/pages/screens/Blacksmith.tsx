import React from 'react'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../../api/player'

const BlacksmithScreen = () => {
  const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const handleClick = () => {
        updatePlayerScreenAsync()
    }

    return (
        <div>
            Blacksmith
            <button onClick={handleClick}>close</button>
        </div>
    )
}

export default BlacksmithScreen