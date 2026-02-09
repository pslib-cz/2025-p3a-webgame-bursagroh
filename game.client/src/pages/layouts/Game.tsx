import React from "react"
import { PlayerIdContext } from "../../providers/global/PlayerIdProvider"
import styles from "./game.module.css"
import NavBar from "../../components/NavBar"
import Inventory from "../../components/Inventory"
import PlayerUI from "../../components/PlayerUI"
import Layer from "../../components/wrappers/layer/Layer"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import IsOpenInventoryProvider from "../../providers/game/IsOpenInventoryProvider"
import InventoryProvider, { InventoryContext } from "../../providers/game/InventoryProvider"
import type { TLoadingWrapperContextState } from '../../types/context'
import useLink from "../../hooks/useLink"
import ProperScreenChecker from "../../components/ProperScreenChecker"

const Game = () => {
    const moveToPage = useLink()

    const playerId = React.useContext(PlayerIdContext)!.playerId

    React.useEffect(() => {
        if (!playerId) {
            moveToPage("root")
        }
    }, [playerId, moveToPage])

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
