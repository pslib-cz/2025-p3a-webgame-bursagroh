import React from "react"
import QueryProvider from "./QueryProvider"
import PlayerIdProvider from "./PlayerIdProvider"
import MineIdProvider from "./MineIdProvider"

const providers = [PlayerIdProvider, MineIdProvider]

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
