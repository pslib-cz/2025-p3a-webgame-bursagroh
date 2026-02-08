import { PlayerIdContext } from "../providers/global/PlayerIdProvider"
import React from "react"
import Link from "../components/Link"
import styles from "./root.module.css"
import Layer from "../components/wrappers/layer/Layer"
import { SaveContext } from "../providers/global/SaveProvider"
import { PlayerContext } from "../providers/global/PlayerProvider"
import useKeyboard from "../hooks/useKeyboard"
import Button from "../components/Button"
import useLink, { screenTypeToPageType } from "../hooks/useLink"
import useBlur from "../hooks/useBlur"

const Root = () => {
    useBlur(true)

    const moveToPage = useLink()

    const playerId = React.useContext(PlayerIdContext)!
    const player = React.useContext(PlayerContext)
    const save = React.useContext(SaveContext)!.save

    const handleClick = async () => {
        await playerId.generatePlayerIdAsync()
        await moveToPage("fountain")
    }

    const handleSave = async () => {
        await save()
        await moveToPage("save")
    }

    useKeyboard("Escape", async () => {
        if (playerId.playerId === null) return

        await moveToPage(screenTypeToPageType(player?.player?.screenType ?? "City"))
    })

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <h1 className={styles.heading}>Urban Relic</h1>
                <div className={styles.linkContainer}>
                    <Link to={screenTypeToPageType(player?.player?.screenType ?? "City")} disabled={playerId.playerId === null}>Continue</Link>
                    <Button onClick={handleClick}>New Game</Button>
                    <Button onClick={handleSave} disabled={playerId.playerId === null}>Save</Button>
                    <Link to="load">Load</Link>
                    <Link to="settings">Settings</Link>
                </div>
            </div>
        </Layer>
    )
}

export default Root
