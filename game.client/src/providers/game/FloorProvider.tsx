import { useQuery } from "@tanstack/react-query"
import React from "react"
import { PlayerIdContext } from "../global/PlayerIdProvider"
import type { Floor } from "../../types/api/models/building"
import { getFloorQuery } from "../../api/building"
import { PlayerContext } from "../global/PlayerProvider"

type FloorContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    floor: Floor | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const FloorContext = React.createContext<FloorContextType | null>(null)

const FloorProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = React.useContext(PlayerContext)!.player!

    const {data: floor, isError, isPending, isSuccess} = useQuery(getFloorQuery(playerId, player.floorId!))

    return <FloorContext.Provider value={{ floor, isError, isPending, isSuccess }}>{children}</FloorContext.Provider>
}

export default FloorProvider   