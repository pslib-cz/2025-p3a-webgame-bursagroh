import type { Save } from "../types/save"

export const parseSave = (save: Save, isAutosave: boolean = false): string => {
    const date = new Date(save.timestamp)
    return `${isAutosave ? "[Autosave]" : ""} ${save.saveString} (${date.toLocaleDateString()} ${date.toLocaleTimeString()})`
}