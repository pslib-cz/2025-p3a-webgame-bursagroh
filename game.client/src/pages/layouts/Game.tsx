import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { Outlet, useLocation } from "react-router"
import { useQuery } from "@tanstack/react-query"
import { getPlayerQuery } from "../../api/player"
import type { ScreenType } from "../../types/api/models/player"
import WrongScreen from "../WrongScreen"
import styles from "./game.module.css"
import NavBar from "../../components/NavBar"
import Inventory from "../../components/Inventory"
import PlayerUI from "../../components/PlayerUI"

// eslint-disable-next-line react-refresh/only-export-components
export const screenTypeToURL = (screenType: ScreenType, level: number | undefined) => {
    switch (screenType) {
        case "City":
            return "/game/city"
        case "Bank":
            return "/game/bank"
        case "Mine":
            return "/game/mine"
        case "Restaurant":
            return "/game/restaurant"
        case "Blacksmith":
            return "/game/blacksmith"
        case "Floor":
            return `/game/floor/${level}`
        case "Fight":
            return "/game/fight"
    }
}

const ProperScreenChecker = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { data, isError, isPending, isSuccess } = useQuery(getPlayerQuery(playerId))
    const location = useLocation()

    if (isError) {
        return <div>Error</div>
    }

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isSuccess) {
        if (screenTypeToURL(data.screenType, data.floor?.level) != location.pathname) {
            return <WrongScreen />
        }

        return <Outlet />
    }
}

const Game = () => {
    const { playerId } = React.useContext(PlayerIdContext)!

    if (playerId === null) {
        return <div>Loading...</div>
    }

    return (
        <div className={styles.container}>
            <NavBar />
            <div className={styles.innerContainer}>
                <ProperScreenChecker />
                <div className={styles.uiContainer}>
                    <PlayerUI />
                    <Inventory />
                </div>
            </div>
        </div>
    )
}

export default Game
