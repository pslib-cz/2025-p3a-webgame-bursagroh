import React from "react"
import QueryProvider from "./QueryProvider"
import PlayerIdProvider from "./PlayerIdProvider"

const Providers: React.FC<React.PropsWithChildren> = ({ children }) => {
    return (
        <>
            {/* Keep always as first provider*/}
            <QueryProvider>
                <PlayerIdProvider>{children}</PlayerIdProvider>
            </QueryProvider>
        </>
    )
}

export default Providers
