import React from 'react'
import useBlur from '../../hooks/useBlur'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { useNavigate } from 'react-router'
import styles from './lose.module.css'
import Button from '../../components/Button'
import useNotification from '../../hooks/useNotification'
import useKeyboard from '../../hooks/useKeyboard'

const LoseScreen = () => {
    useBlur(true)

    const navigate = useNavigate()
    const {genericError} = useNotification()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City", genericError))

    const handleClick = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    useKeyboard("Escape", () => {
        navigate("/")
    })

    return (
        <div className={styles.container}>
            <span className={styles.heading}>You Died</span>
            <Button onClick={handleClick}>Respawn</Button>
        </div>
    )
}

export default LoseScreen