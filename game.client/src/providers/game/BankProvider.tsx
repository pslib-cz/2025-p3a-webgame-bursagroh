import { useQuery } from "@tanstack/react-query"
import React from "react"
import { PlayerIdContext } from "../global/PlayerIdProvider"
import type { InventoryItem } from "../../types/api/models/player"
import { getBankInventoryQuery } from "../../api/bank"

type BankContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    bank: Array<InventoryItem> | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const BankContext = React.createContext<BankContextType | null>(null)

const BankProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const {data: bank, isError, isPending, isSuccess} = useQuery(getBankInventoryQuery(playerId))

    return <BankContext.Provider value={{ bank, isError, isPending, isSuccess }}>{children}</BankContext.Provider>
}

export default BankProvider   