import React from 'react'
import useBlur from '../../hooks/useBlur'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useNavigate } from 'react-router'

const LoseScreen = () => {
    useBlur(true)

    const navigate = useNavigate()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const handleClick = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    return (
        <div>
            <span>You Died</span>
            <button onClick={handleClick}>Respawn</button>
        </div>
    )
}

export default LoseScreen