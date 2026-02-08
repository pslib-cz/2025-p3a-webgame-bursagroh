import React from 'react'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import useBlur from '../../hooks/useBlur'
import styles from './win.module.css'
import Link from '../../components/Link'
import { useNavigate } from 'react-router'
import useKeyboard from '../../hooks/useKeyboard'

const WinScreen = () => {
    useBlur(true)

    const navigate = useNavigate()

    const playerId = React.useContext(PlayerIdContext)!

    const handleClick = () => {
        playerId.generatePlayerIdAsync()
    }

    useKeyboard("Escape", () => {
        navigate("/")
    })

    return (
        <div className={styles.container}>
            <span className={styles.heading}>Win</span>
            <Link to="/game/city" onClick={handleClick}>New Game</Link>
        </div>
    )
}

export default WinScreen