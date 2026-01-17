import React from "react"

type ActiveItemContextType = {
    activeItemInventoryItemId: number | null
    setActiveItemInventoryItemId: React.Dispatch<React.SetStateAction<number | null>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const ActiveItemContext = React.createContext<ActiveItemContextType | null>(null)

const ActiveItemProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [activeItemInventoryItemId, setActiveItemInventoryItemId] = React.useState<number | null>(null)

    return <ActiveItemContext.Provider value={{ activeItemInventoryItemId, setActiveItemInventoryItemId }}>{children}</ActiveItemContext.Provider>
}

export default ActiveItemProvider
