import React from "react"
import type { MapType } from "../types/map"

type MapContextType = {
    mapType: MapType
    setMapType: React.Dispatch<React.SetStateAction<MapType>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const MapContext = React.createContext<MapContextType | null>(null)

const MapProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [mapType, setMapType] = React.useState<MapType>("city")

    return <MapContext.Provider value={{ mapType, setMapType }}>{children}</MapContext.Provider>
}

export default MapProvider