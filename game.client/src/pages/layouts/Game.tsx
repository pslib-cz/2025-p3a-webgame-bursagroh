import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { Outlet, useLocation, useNavigate } from "react-router"
import { useQuery } from "@tanstack/react-query"
import { getPlayerQuery } from "../../api/player"
import type { ScreenType } from "../../types/api/models/player"
import styles from "./game.module.css"
import NavBar from "../../components/NavBar"
import Inventory from "../../components/Inventory"
import PlayerUI from "../../components/PlayerUI"
import GameProviders from "../../providers/game"
import Layer from "../../components/wrappers/layer/Layer"
import WrongScreen from "../WrongScreen"

// eslint-disable-next-line react-refresh/only-export-components
export const screenTypeToURL = (screenType: ScreenType) => {
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
            return `/game/floor`
        case "Fight":
            return "/game/fight"
        case "Fountain":
            return "/game/fountain"
    }
}

const ProperScreenChecker = () => {
    const navigate = useNavigate()
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
        if (screenTypeToURL(data.screenType) != location.pathname) {
            navigate(screenTypeToURL(data.screenType)!)
            return <WrongScreen />
        }

        return <Outlet />
    }
}

const Game = () => {
    const { playerId } = React.useContext(PlayerIdContext)!

    if (playerId === null) {
        return (
            <Layer layer={1}>
                <div>Loading...</div>
            </Layer>
        )
    }

    return (
        <GameProviders>
            <Layer layer={1}>
                <NavBar />
                <ProperScreenChecker />
            </Layer>
            <Layer layer={2}>
                <div className={styles.uiContainer}>
                    <PlayerUI />
                    <Inventory />
                </div>
            </Layer>
        </GameProviders>
    )
}

export default Game
