import React from "react"
import { PlayerIdContext } from "../providers/PlayerIdProvider"
import ScreenDisplay from "../components/ScreenDisplay"

const Game = () => {
    const {playerId} = React.useContext(PlayerIdContext)!

    if (playerId === null) {
        return <div>Loading...</div>
    }

    return (
        <>
            <div>Player ID: {playerId}</div>
            <ScreenDisplay />
        </>
    )
}

export default Game
