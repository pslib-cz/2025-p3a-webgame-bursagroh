import React from "react"
import InventoryProvider, { InventoryContext } from "./InventoryProvider"
import IsOpenInventoryProvider from "./IsOpenInventoryProvider"
import BankProvider, { BankContext } from "./BankProvider"
import RecipesProvider, { RecipesContext } from "./RecipesProvider"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import type { TLoadingWrapperContextState } from "../../components/wrappers/LoadingWrapper"
import LeaderboardProvider, { LeaderboardContext } from "./LeaderboardProvider"

const providers = [InventoryProvider, IsOpenInventoryProvider, BankProvider, RecipesProvider, LeaderboardProvider]
const contextsToLoad = [InventoryContext, BankContext, RecipesContext, LeaderboardContext] as Array<React.Context<TLoadingWrapperContextState>>

const GameProviders: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <ProviderGroupLoadingWrapper providers={providers} contextsToLoad={contextsToLoad}>
            {children}
        </ProviderGroupLoadingWrapper>
    )
}

export default GameProviders
