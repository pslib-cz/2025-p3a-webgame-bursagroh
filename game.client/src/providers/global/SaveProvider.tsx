import { useMutation } from "@tanstack/react-query"
import React from "react"
import { saveMutation } from "../../api/save"
import { PlayerIdContext } from "./PlayerIdProvider"
import useStorage from "../../hooks/useStorage"
import { SettingsContext } from "./SettingsProvider"

type SaveState = "idle" | "saving" | "saved"

type SaveContextType = {
    saves: SavesStorage
    saveState: SaveState
    saveString: string | null
    save: (isAutosave?: boolean) => Promise<void>
}

type SavesStorage = {
    autosaves: Save[],
    saves: Save[]
}

export type Save = {
    saveString: string,
    timestamp: string
}

// eslint-disable-next-line react-refresh/only-export-components
export const SaveContext = React.createContext<SaveContextType | null>(null)

const SaveProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const autosave = React.useContext(SettingsContext)!

    const [saveString, setSaveString] = React.useState<string | null>(null)
    const [saveState, setSaveState] = React.useState<SaveState>("idle")
    const [isAutosave, setIsAutosave] = React.useState(false)

    const [saves, setSaves] = useStorage<SavesStorage>("saves", {
        autosaves: [],
        saves: []
    })

    const {mutateAsync: saveAsync} = useMutation(saveMutation(playerId, (data) => {
        setSaveString(data.saveString)

        const newSave: Save = {
            saveString: data.saveString,
            timestamp: new Date().toISOString()
        }

        if (!isAutosave) {
            setSaves((prev) => {
                const newSaves = {
                    ...prev,
                    saves: [newSave, ...prev.saves]
                }
                return newSaves
            })
        } else {
            setSaves((prev) => {
                const newSaves = {
                    ...prev,
                    autosaves: [newSave, ...prev.autosaves].slice(0, autosave.maxAutosave)
                }
                return newSaves
            })
        }
    }))

    const save = async (isAutosave?: boolean) => {
        if (saveState === "saving") return

        setSaveState("saving")
        setIsAutosave(!!isAutosave)

        await saveAsync()

        setSaveState("saved")

        setTimeout(() => {
            setSaveState("idle")
        }, 10000)
    }

    React.useEffect(() => {
        const intervalMs = autosave.autosaveInterval * 60000

        const id = window.setInterval(async () => {
            await save(true)
        }, intervalMs)

        return () => window.clearInterval(id)
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [autosave.autosaveInterval])

    return <SaveContext.Provider value={{ saves, save, saveString, saveState }}>{children}</SaveContext.Provider>
}

export default SaveProvider
