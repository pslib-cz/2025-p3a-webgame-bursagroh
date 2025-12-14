import React from "react"
import { useQuery } from "@tanstack/react-query"
import { generateMineQuery } from "../api/mine"

type MineIdContextType = {
    mineId: number | null
}

// eslint-disable-next-line react-refresh/only-export-components
export const MineIdContext = React.createContext<MineIdContextType | null>(null)

const MineIdProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const { data, isPending, isError } = useQuery(generateMineQuery())

    if (isError) {
        return <div>ERROR: Generating mine</div>
    }

    if (isPending) {
        return <div>Generating mine...</div>
    }

    return <MineIdContext.Provider value={{ mineId: data.mineId }}>{children}</MineIdContext.Provider>
}

export default MineIdProvider
