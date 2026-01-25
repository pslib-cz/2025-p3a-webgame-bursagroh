import { PlayerIdContext } from "../providers/PlayerIdProvider"
import React from "react"
import Link from "../components/Link"
import styles from "./root.module.css"

const Root = () => {
    const playerId = React.useContext(PlayerIdContext)!

    const handleClick = () => {
        playerId.generatePlayerIdAsync()
    }

    return (
        <div className={styles.container}>
            <h1 className={styles.heading}>Urban Relic</h1>
            <div className={styles.linkContainer}>
                <Link to="/game/city" onClick={handleClick} disabled={playerId.playerId === null}>Continue</Link>
                <Link to="/game/city" onClick={handleClick}>New Game</Link>
                <Link to="/save" onClick={handleClick}>Save</Link>
                <Link to="/load" onClick={handleClick}>Load</Link>
                <Link to="/settings" onClick={handleClick}>Settings</Link>
            </div>
        </div>
    )
}

export default Root
