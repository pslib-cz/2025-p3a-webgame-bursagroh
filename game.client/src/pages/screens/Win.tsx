import React from 'react'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import useBlur from '../../hooks/useBlur'
import styles from './win.module.css'
import Link from '../../components/Link'

const WinScreen = () => {
    useBlur(true)

    const playerId = React.useContext(PlayerIdContext)!

    const handleClick = () => {
        playerId.generatePlayerIdAsync()
    }

    return (
        <div className={styles.container}>
            <span className={styles.heading}>Win</span>
            <Link to="/game/city" onClick={handleClick}>New Game</Link>
        </div>
    )
}

export default WinScreen