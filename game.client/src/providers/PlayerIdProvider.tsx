import React from "react"
import { generatePlayerMutation } from "../api/player"
import { useMutation } from "@tanstack/react-query"

type PlayerIdContextType = {
    playerId: string | null
    generatePlayerIdAsync: () => Promise<void>
}

// eslint-disable-next-line react-refresh/only-export-components
export const PlayerIdContext = React.createContext<PlayerIdContextType | null>(null)

const PlayerIdProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [playerId, setPlayerId] = React.useState<string | null>(null)

    const { mutateAsync: generatePlayerAsync, isPending, isError } = useMutation(generatePlayerMutation())

    const generatePlayerIdAsync = React.useCallback(async () => {
        const data = await generatePlayerAsync()

        setPlayerId(data.playerId)
    }, [generatePlayerAsync])

    if (isError) {
        return <div>ERROR: Generating player</div>
    }

    if (isPending) {
        return <div>Generating player...</div>
    }

    return <PlayerIdContext.Provider value={{ playerId, generatePlayerIdAsync }}>{children}</PlayerIdContext.Provider>
}

export default PlayerIdProvider
