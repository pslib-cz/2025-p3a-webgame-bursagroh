import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getPlayerInventoryQuery } from "../../api/player"
import { PlayerIdContext } from "../global/PlayerIdProvider"
import type { InventoryItem } from "../../types/api/models/player"

type InventoryContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    inventory: Array<InventoryItem> | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const InventoryContext = React.createContext<InventoryContextType | null>(null)

const InventoryProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const {data: inventory, isError, isPending, isSuccess} = useQuery(getPlayerInventoryQuery(playerId))

    return <InventoryContext.Provider value={{ inventory, isError, isPending, isSuccess }}>{children}</InventoryContext.Provider>
}

export default InventoryProvider   