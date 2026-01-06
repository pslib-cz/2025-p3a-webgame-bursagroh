import React from "react"
import QueryProvider from "./QueryProvider"
import PlayerIdProvider from "./PlayerIdProvider"

const providers = [PlayerIdProvider]

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
