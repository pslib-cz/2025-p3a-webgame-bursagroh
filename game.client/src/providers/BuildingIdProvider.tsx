import React from "react"

type BuildingIdContextType = {
    buildingId: number | null
    setBuildingId: React.Dispatch<React.SetStateAction<number | null>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const BuildingIdContext = React.createContext<BuildingIdContextType | null>(null)

const BuildingIdProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [buildingId, setBuildingId] = React.useState<number | null>(null)

    return <BuildingIdContext.Provider value={{ buildingId, setBuildingId }}>{children}</BuildingIdContext.Provider>
}

export default BuildingIdProvider
