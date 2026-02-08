import React from "react"
import { useMutation } from "@tanstack/react-query"
import { queryClient } from "../../api"
import { generatePlayerMutation, getPlayerQuery } from "../../api/player"
import styles from "./playerIdProvider.module.css"

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
    const [isRestoring, setIsRestoring] = React.useState(true)
    const { mutateAsync: generatePlayerAsync, isPending, isError } = useMutation(generatePlayerMutation())

    React.useEffect(() => {
        let isMounted = true

        const restorePlayerId = async () => {
            try {
                const raw = localStorage.getItem(PLAYER_ID_STORAGE_KEY)
                if (!raw) return

                const parsed = JSON.parse(raw) as { id: string; exp: number }
                if (parsed.exp <= Date.now()) {
                    localStorage.removeItem(PLAYER_ID_STORAGE_KEY)
                    return
                }

                await queryClient.fetchQuery(getPlayerQuery(parsed.id))
                if (isMounted) {
                    setPlayerId(parsed.id)
                    const payload = { id: parsed.id, exp: Date.now() + ONE_HOUR_MS }
                    localStorage.setItem(PLAYER_ID_STORAGE_KEY, JSON.stringify(payload))
                }
            } catch {
                localStorage.removeItem(PLAYER_ID_STORAGE_KEY)
            } finally {
                if (isMounted) {
                    setIsRestoring(false)
                }
            }
        }

        restorePlayerId()

        return () => {
            isMounted = false
        }
    }, [])

    React.useEffect(() => {
        if (isRestoring) return

        if (playerId == null) {
            localStorage.removeItem(PLAYER_ID_STORAGE_KEY)
            return
        }

        const payload = { id: playerId, exp: Date.now() + ONE_HOUR_MS }
        localStorage.setItem(PLAYER_ID_STORAGE_KEY, JSON.stringify(payload))
    }, [isRestoring, playerId])

    const generatePlayerIdAsync = React.useCallback(async () => {
        const data = await generatePlayerAsync()
        setPlayerId(data.playerId)
    }, [generatePlayerAsync])

    if (isRestoring) {
        return <span className={styles.loading}>Restoring player...</span>
    }

    if (isError) {
        return <span className={styles.loading}>Error generating player!</span>
    }

    if (isPending) {
        return <span className={styles.loading}>Generating player...</span>
    }

    return <PlayerIdContext.Provider value={{ playerId, setPlayerId, generatePlayerIdAsync }}>{children}</PlayerIdContext.Provider>
}

export default PlayerIdProvider
