import React from "react"
import useStorage from "../../hooks/useStorage"
import { DEFAULT_AUTOSAVE_INTERVAL, DEFAULT_MAX_AUTOSAVE } from "../../constants/settings"

type SettingsContextType = {
    autosaveInterval: number
    setAutosaveInterval: React.Dispatch<React.SetStateAction<number>>
    maxAutosave: number
    setMaxAutosave: React.Dispatch<React.SetStateAction<number>>
    deleteAutosaveInterval: () => void
    deleteMaxAutosave: () => void
}

// eslint-disable-next-line react-refresh/only-export-components
export const SettingsContext = React.createContext<SettingsContextType | null>(null)

const SettingsProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [autosaveInterval, setAutosaveInterval, deleteAutosaveInterval] = useStorage("autosaveInterval", DEFAULT_AUTOSAVE_INTERVAL)
    const [maxAutosave, setMaxAutosave, deleteMaxAutosave] = useStorage("maxAutosave", DEFAULT_MAX_AUTOSAVE)

    return <SettingsContext.Provider value={{ autosaveInterval, setAutosaveInterval, maxAutosave, setMaxAutosave, deleteAutosaveInterval, deleteMaxAutosave }}>{children}</SettingsContext.Provider>
}

export default SettingsProvider
