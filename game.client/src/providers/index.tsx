import React from "react"
import QueryProvider from "./QueryProvider"
import PlayerIdProvider from "./PlayerIdProvider"
import ActivePickaxeProvider from "./ActivePickaxeProvider"

const providers = [PlayerIdProvider, ActivePickaxeProvider]

const Providers: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <>
            <QueryProvider>
                {providers.reduce(
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
