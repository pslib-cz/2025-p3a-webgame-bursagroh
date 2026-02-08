import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getMineItemsQuery } from "../../api/mine"
import type { MineItem } from "../../types/api/models/mine"
import { PlayerContext } from "../global/PlayerProvider"

type MineItemsContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    mineItems: Array<MineItem> | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const MineItemsContext = React.createContext<MineItemsContextType | null>(null)

const MineItemsProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const player = React.useContext(PlayerContext)!.player!

    const {data: mineItems, isError, isPending, isSuccess} = useQuery(getMineItemsQuery(player.playerId, player.mineId))

    return <MineItemsContext.Provider value={{ mineItems, isError, isPending, isSuccess }}>{children}</MineItemsContext.Provider>
}

export default MineItemsProvider   