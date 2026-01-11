import React from "react"

type MineIdContextType = {
    mineId: number | null
    setMineId: React.Dispatch<React.SetStateAction<number | null>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const MineIdContext = React.createContext<MineIdContextType | null>(null)

const MineIdProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [mineId, setMineId] = React.useState<number | null>(null)

    return <MineIdContext.Provider value={{ mineId, setMineId }}>{children}</MineIdContext.Provider>
}

export default MineIdProvider
