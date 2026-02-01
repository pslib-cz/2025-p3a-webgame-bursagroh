import React from "react"
import QueryProvider from "./QueryProvider"
import PlayerIdProvider from "./PlayerIdProvider"
import ActiveItemProvider from "./ActiveItemProvider"
import MineIdProvider from "./MineIdProvider"
import BuildingIdProvider from "./BuildingIdProvider"
import LayerProvider from "./LayerProvider"

const providers = [PlayerIdProvider, MineIdProvider, BuildingIdProvider, LayerProvider, ActiveItemProvider]

const Providers: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <>
            <QueryProvider>
                {providers.reduceRight(
                    (acc, Provider) => (
                        <Provider>{acc}</Provider>
                    ),
                    children
                )}
            </QueryProvider>
        </>
    )
}

export default Providers
