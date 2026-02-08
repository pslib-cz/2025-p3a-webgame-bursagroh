import React from "react"
import QueryProvider from "./global/QueryProvider"
import PlayerIdProvider from "./global/PlayerIdProvider"
import PlayerProvider, { PlayerContext } from "./global/PlayerProvider"
import IsBluredProvider from "./global/IsBluredProvider"
import MapProvider from "./global/MapProvider"
import ProviderGroupLoadingWrapper from "../components/wrappers/ProviderGroupLoadingWrapper"
import type { TLoadingWrapperContextState } from '../types/context'
import SettingsProvider from "./global/SettingsProvider"
import SaveProvider from "./global/SaveProvider"
import NotificationProvider from "./global/NotificationProvider"
import TooltipProvider from "./global/TooltipProvider"

const Providers: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <>
            <QueryProvider>
                <ProviderGroupLoadingWrapper providers={[PlayerIdProvider, IsBluredProvider, MapProvider, SettingsProvider, NotificationProvider, TooltipProvider]} contextsToLoad={[]}>
                    <ProviderGroupLoadingWrapper providers={[SaveProvider, PlayerProvider]} contextsToLoad={[PlayerContext] as Array<React.Context<TLoadingWrapperContextState>>}>
                        {children}
                    </ProviderGroupLoadingWrapper>
                </ProviderGroupLoadingWrapper>
            </QueryProvider>
        </>
    )
}

export default Providers
