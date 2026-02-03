import React from "react"
import InventoryProvider, { InventoryContext } from "./InventoryProvider"
import IsOpenInventoryProvider from "./IsOpenInventoryProvider"
import BankProvider, { BankContext } from "./BankProvider"
import RecipesProvider, { RecipesContext } from "./RecipesProvider"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import type { TLoadingWrapperContextState } from "../../components/wrappers/LoadingWrapper"

const providers = [InventoryProvider, IsOpenInventoryProvider, BankProvider, RecipesProvider]
const contextsToLoad = [InventoryContext, BankContext, RecipesContext] as Array<React.Context<TLoadingWrapperContextState>>

const GameProviders: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <ProviderGroupLoadingWrapper providers={providers} contextsToLoad={contextsToLoad}>
            {children}
        </ProviderGroupLoadingWrapper>
    )
}

export default GameProviders
