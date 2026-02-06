import { useQuery } from "@tanstack/react-query"
import React from "react"
import type { LeaderboardEntry } from "../../types/api/models/recipe"
import { getLeaderboardQuery } from "../../api/recipe"

type LeaderboardContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    leaderboard: LeaderboardEntry[] | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const LeaderboardContext = React.createContext<LeaderboardContextType | null>(null)

const LeaderboardProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const {data: leaderboard, isError, isPending, isSuccess} = useQuery(getLeaderboardQuery())

    return <LeaderboardContext.Provider value={{ leaderboard, isError, isPending, isSuccess }}>{children}</LeaderboardContext.Provider>
}

export default LeaderboardProvider   