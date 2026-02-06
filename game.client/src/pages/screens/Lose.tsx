import React from 'react'
import useBlur from '../../hooks/useBlur'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useNavigate } from 'react-router'
import styles from './lose.module.css'
import Button from '../../components/Button'

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
        <div className={styles.container}>
            <span className={styles.heading}>You Died</span>
            <Button onClick={handleClick}>Respawn</Button>
        </div>
    )
}

export default LoseScreen