import React from "react"
import PlayerProvider, { PlayerContext } from "./PlayerProvider"
import InventoryProvider, { InventoryContext } from "./InventoryProvider"
import LoadingWrapper, { type TLoadingWrapperContextState } from "../../components/wrappers/LoadingWrapper"
import IsOpenInventoryProvider from "./IsOpenInventoryProvider"
import BankProvider, { BankContext } from "./BankProvider"
import RecipesProvider, { RecipesContext } from "./RecipesProvider"
import FloorProvider, { FloorContext } from "./FloorProvider"

const providers = [PlayerProvider, InventoryProvider, IsOpenInventoryProvider, BankProvider, RecipesProvider, FloorProvider]
const contextsToLoad = [PlayerContext, InventoryContext, BankContext, RecipesContext, FloorContext]

const GameProviders: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <>
            {providers.reduceRight(
                (acc, Provider) => (
                    <Provider>{acc}</Provider>
                ),
                contextsToLoad.reduceRight(
                    (acc, context) => (
                        <LoadingWrapper context={context as React.Context<TLoadingWrapperContextState>}>{acc}</LoadingWrapper>
                    ),
                    children
                )
            )}
        </>
    )
}

export default GameProviders
