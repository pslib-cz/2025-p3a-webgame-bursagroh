import React from "react"
import { PlayerIdContext } from "../../providers/global/PlayerIdProvider"
import { Outlet, useLocation, useNavigate } from "react-router"
import type { ScreenType } from "../../types/api/models/player"
import styles from "./game.module.css"
import NavBar from "../../components/NavBar"
import Inventory from "../../components/Inventory"
import PlayerUI from "../../components/PlayerUI"
import Layer from "../../components/wrappers/layer/Layer"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import IsOpenInventoryProvider from "../../providers/game/IsOpenInventoryProvider"
import InventoryProvider, { InventoryContext } from "../../providers/game/InventoryProvider"
import type { TLoadingWrapperContextState } from "../../components/wrappers/LoadingWrapper"
import { PlayerContext } from "../../providers/global/PlayerProvider"

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
        case "Win":
            return "/game/win"
        case "Lose":
            return "/game/lose"
    }
}

const ProperScreenChecker = () => {
    const navigate = useNavigate()
    const location = useLocation()

    const player = React.useContext(PlayerContext)!.player!
    

    React.useEffect(() => {
        if (screenTypeToURL(player.screenType) != location.pathname) {
            navigate(screenTypeToURL(player.screenType)!)
        }
    }, [player.screenType, navigate, location.pathname])

    if (screenTypeToURL(player.screenType) != location.pathname) {
        return null
    }

    return <Outlet />
}

const Game = () => {
    const navigate = useNavigate()

    const playerId = React.useContext(PlayerIdContext)!.playerId

    React.useEffect(() => {
        if (!playerId) {
            navigate("/")
        }
    }, [playerId, navigate])

    return (
        <ProviderGroupLoadingWrapper providers={[InventoryProvider, IsOpenInventoryProvider]} contextsToLoad={[InventoryContext] as Array<React.Context<TLoadingWrapperContextState>>}>
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
        </ProviderGroupLoadingWrapper>
    )
}

export default Game
