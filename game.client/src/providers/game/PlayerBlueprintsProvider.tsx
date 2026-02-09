import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getPlayerBlueprintsQuery } from "../../api/blueprint"
import type { Blueprint } from "../../types/api/models/blueprint"
import { PlayerIdContext } from "../global/PlayerIdProvider"

type PlayerBlueprintsContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    blueprints: Blueprint[] | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const PlayerBlueprintsContext = React.createContext<PlayerBlueprintsContextType | null>(null)

const PlayerBlueprintsProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const {data: blueprints, isError, isPending, isSuccess} = useQuery(getPlayerBlueprintsQuery(playerId))

    return <PlayerBlueprintsContext.Provider value={{ blueprints, isError, isPending, isSuccess }}>{children}</PlayerBlueprintsContext.Provider>
}

export default PlayerBlueprintsProvider   