import React from 'react'
import { Link } from 'react-router'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import useBlur from '../../hooks/useBlur'

const WinScreen = () => {
    useBlur(true)

    const playerId = React.useContext(PlayerIdContext)!

    const handleClick = () => {
        playerId.generatePlayerIdAsync()
    }

    return (
        <div>
            <span>Win</span>
            <Link to="/game/city" onClick={handleClick}>New Game</Link>
        </div>
    )
}

export default WinScreen