import React from "react"

type LayerContextType = {
    layer: number | null
    setLayer: React.Dispatch<React.SetStateAction<number | null>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const LayerContext = React.createContext<LayerContextType | null>(null)

const LayerProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [layer, setLayer] = React.useState<number | null>(null)

    return <LayerContext.Provider value={{ layer, setLayer }}>{children}</LayerContext.Provider>
}

export default LayerProvider
