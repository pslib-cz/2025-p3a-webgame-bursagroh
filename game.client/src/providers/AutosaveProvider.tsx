import React from "react"
import useStorage from "../hooks/useStorage"
import { DEFAULT_AUTOSAVE_INTERVAL, DEFAULT_MAX_AUTOSAVE } from "../constants/settings"

type AutosaveContextType = {
    autosaveInterval: number
    setAutosaveInterval: React.Dispatch<React.SetStateAction<number>>
    maxAutosave: number
    setMaxAutosave: React.Dispatch<React.SetStateAction<number>>
    deleteAutosaveInterval: () => void
    deleteMaxAutosave: () => void
}

// eslint-disable-next-line react-refresh/only-export-components
export const AutosaveContext = React.createContext<AutosaveContextType | null>(null)

const AutosaveProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [autosaveInterval, setAutosaveInterval, deleteAutosaveInterval] = useStorage("autosaveInterval", DEFAULT_AUTOSAVE_INTERVAL)
    const [maxAutosave, setMaxAutosave, deleteMaxAutosave] = useStorage("maxAutosave", DEFAULT_MAX_AUTOSAVE)

    return <AutosaveContext.Provider value={{ autosaveInterval, setAutosaveInterval, maxAutosave, setMaxAutosave, deleteAutosaveInterval, deleteMaxAutosave }}>{children}</AutosaveContext.Provider>
}

export default AutosaveProvider
