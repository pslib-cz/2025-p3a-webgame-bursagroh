import React from "react"
import QueryProvider from "./QueryProvider"
import PlayerIdProvider from "./PlayerIdProvider"
import ActiveItemProvider from "./ActiveItemProvider"
import MineIdProvider from "./MineIdProvider"
import BuildingIdProvider from "./BuildingIdProvider"
import LayerProvider from "./LayerProvider"
import PlayerProvider, { PlayerContext } from "./game/PlayerProvider"
import IsBluredProvider from "./IsBluredProvider"
import MapProvider from "./MapProvider"
import FloorProvider, { FloorContext } from "./FloorProvider"
import ProviderGroupLoadingWrapper from "../components/wrappers/ProviderGroupLoadingWrapper"
import type { TLoadingWrapperContextState } from "../components/wrappers/LoadingWrapper"
import AutosaveProvider from "./AutosaveProvider"
import SaveProvider from "./SaveProvider"

const providers = [PlayerIdProvider, PlayerProvider, MineIdProvider, BuildingIdProvider, LayerProvider, ActiveItemProvider, IsBluredProvider, MapProvider, AutosaveProvider]
const contextsToLoad = [PlayerContext] as Array<React.Context<TLoadingWrapperContextState>>

const Providers: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <>
            <QueryProvider>
                <ProviderGroupLoadingWrapper providers={providers} contextsToLoad={contextsToLoad}>
                    <ProviderGroupLoadingWrapper providers={[FloorProvider, SaveProvider]} contextsToLoad={[FloorContext] as Array<React.Context<TLoadingWrapperContextState>>}>
                        {children}
                    </ProviderGroupLoadingWrapper>
                </ProviderGroupLoadingWrapper>
            </QueryProvider>
        </>
    )
}

export default Providers
