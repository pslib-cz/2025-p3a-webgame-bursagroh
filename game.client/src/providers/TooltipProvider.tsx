import React from "react"

type Tooltip = {
    mouseX: number
    mouseY: number
    relativeX: number
    relativeY: number
    heading: string
    text: string
    id: string
}

type TooltipContextType = {
    tooltip: Tooltip | null
    setTooltip: React.Dispatch<React.SetStateAction<Tooltip | null>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const TooltipContext = React.createContext<TooltipContextType | null>(null)

const TooltipProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [tooltip, setTooltip] = React.useState<Tooltip | null>(null)

    return <TooltipContext.Provider value={{ tooltip, setTooltip }}>{children}</TooltipContext.Provider>
}

export default TooltipProvider
