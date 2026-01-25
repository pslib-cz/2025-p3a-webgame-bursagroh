import React from "react"
import { generatePlayerMutation } from "../api/player"
import { useMutation } from "@tanstack/react-query"

const PLAYER_ID_STORAGE_KEY = "playerId"
const ONE_HOUR_MS = 60 * 60 * 1000

type PlayerIdContextType = {
    playerId: string | null
    setPlayerId: React.Dispatch<React.SetStateAction<string | null>>
    generatePlayerIdAsync: () => Promise<void>
}

// eslint-disable-next-line react-refresh/only-export-components
export const PlayerIdContext = React.createContext<PlayerIdContextType | null>(null)

const PlayerIdProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [playerId, setPlayerId] = React.useState<string | null>(null)
    const { mutateAsync: generatePlayerAsync, isPending, isError } = useMutation(generatePlayerMutation())

    React.useEffect(() => {
        const restorePlayerId = () => {
            try {
                const raw = localStorage.getItem(PLAYER_ID_STORAGE_KEY)
                if (!raw) return
                const parsed = JSON.parse(raw) as { id: string; exp: number }
                if (parsed.exp > Date.now()) {
                    setPlayerId(parsed.id)
                } else {
                    localStorage.removeItem(PLAYER_ID_STORAGE_KEY)
                }
            } catch {
                localStorage.removeItem(PLAYER_ID_STORAGE_KEY)
            }
        }
        restorePlayerId()
    }, [])

    React.useEffect(() => {
        if (playerId == null) {
            localStorage.removeItem(PLAYER_ID_STORAGE_KEY)
            return
        }
        const payload = { id: playerId, exp: Date.now() + ONE_HOUR_MS }
        localStorage.setItem(PLAYER_ID_STORAGE_KEY, JSON.stringify(payload))
    }, [playerId])

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

    return <PlayerIdContext.Provider value={{ playerId, setPlayerId, generatePlayerIdAsync }}>{children}</PlayerIdContext.Provider>
}

export default PlayerIdProvider
