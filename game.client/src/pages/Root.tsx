import { PlayerIdContext } from "../providers/PlayerIdProvider"
import React from "react"
import Link from "../components/Link"
import styles from "./root.module.css"
import Layer from "../components/wrappers/layer/Layer"
import useBlur from "../hooks/useBlur"

const Root = () => {
    useBlur(true)

    const playerId = React.useContext(PlayerIdContext)!
    
    const handleClick = () => {
        playerId.generatePlayerIdAsync()
    }

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <h1 className={styles.heading}>Urban Relic</h1>
                <div className={styles.linkContainer}>
                    <Link to="/game/city" disabled={playerId.playerId === null}>Continue</Link>
                    <Link to="/game/city" onClick={handleClick}>New Game</Link>
                    <Link to="/save">Save</Link>
                    <Link to="/load">Load</Link>
                    <Link to="/settings">Settings</Link>
                </div>
            </div>
        </Layer>
    )
}

export default Root
