import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getPlayerQuery } from "../../api/player"
import { PlayerIdContext } from "./PlayerIdProvider"
import type { Player } from "../../types/api/models/player"

type PlayerContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    player: Player | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const PlayerContext = React.createContext<PlayerContextType | null>(null)

const InnerPlayerProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const {data: player, isError, isPending, isSuccess} = useQuery(getPlayerQuery(playerId))

    return <PlayerContext.Provider value={{ player, isError, isPending, isSuccess }}>{children}</PlayerContext.Provider>
}

const PlayerProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId
    

    if (!playerId) {
        return <PlayerContext.Provider value={null}>{children}</PlayerContext.Provider>
    }

    return <InnerPlayerProvider>{children}</InnerPlayerProvider>
}

export default PlayerProvider   