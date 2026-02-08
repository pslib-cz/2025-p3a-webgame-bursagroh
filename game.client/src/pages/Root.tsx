import { PlayerIdContext } from "../providers/global/PlayerIdProvider"
import React from "react"
import Link from "../components/Link"
import styles from "./root.module.css"
import Layer from "../components/wrappers/layer/Layer"
import useBlur from "../hooks/useBlur"
import { SaveContext } from "../providers/global/SaveProvider"
import { PlayerContext } from "../providers/global/PlayerProvider"
import { screenTypeToURL } from "./layouts/Game"
import { useNavigate } from "react-router"
import useKeyboard from "../hooks/useKeyboard"
import Button from "../components/Button"

const Root = () => {
    useBlur(true)

    const navigate = useNavigate()

    const playerId = React.useContext(PlayerIdContext)!
    const player = React.useContext(PlayerContext)
    const save = React.useContext(SaveContext)!.save

    const handleClick = async () => {
        await playerId.generatePlayerIdAsync()
        navigate("/game/city")
    }

    const handleSave = async () => {
        await save()
        navigate("/save")
    }

    useKeyboard("Escape", () => {
        if (playerId.playerId === null) return
        
        navigate(screenTypeToURL(player?.player?.screenType ?? "City"))
    })

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <h1 className={styles.heading}>Urban Relic</h1>
                <div className={styles.linkContainer}>
                    <Link to={screenTypeToURL(player?.player?.screenType ?? "City")} disabled={playerId.playerId === null}>Continue</Link>
                    <Button onClick={handleClick}>New Game</Button>
                    <Button onClick={handleSave} disabled={playerId.playerId === null}>Save</Button>
                    <Link to="/load">Load</Link>
                    <Link to="/settings">Settings</Link>
                </div>
            </div>
        </Layer>
    )
}

export default Root
