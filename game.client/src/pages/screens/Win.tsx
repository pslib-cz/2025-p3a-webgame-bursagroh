import React from 'react'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import useBlur from '../../hooks/useBlur'
import styles from './win.module.css'
import { useNavigate } from 'react-router'
import useKeyboard from '../../hooks/useKeyboard'
import Button from '../../components/Button'

const WinScreen = () => {
    useBlur(true)

    const navigate = useNavigate()

    const playerId = React.useContext(PlayerIdContext)!

    const handleClick = async () => {
        await playerId.generatePlayerIdAsync()
        navigate("/game/fountain")
    }

    useKeyboard("Escape", () => {
        navigate("/")
    })

    return (
        <div className={styles.container}>
            <span className={styles.heading}>Win</span>
            <Button onClick={handleClick}>New Game</Button>
        </div>
    )
}

export default WinScreen