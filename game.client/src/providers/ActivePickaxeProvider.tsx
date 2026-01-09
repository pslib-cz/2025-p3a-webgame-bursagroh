import React from "react"

type ActivePickaxeContextType = {
    activePickaxeInventoryItemId: number | null
    setActivePickaxeInventoryItemId: React.Dispatch<React.SetStateAction<number | null>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const ActivePickaxeContext = React.createContext<ActivePickaxeContextType | null>(null)

const ActivePickaxeProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [activePickaxeInventoryItemId, setActivePickaxeInventoryItemId] = React.useState<number | null>(null)

    return <ActivePickaxeContext.Provider value={{ activePickaxeInventoryItemId, setActivePickaxeInventoryItemId }}>{children}</ActivePickaxeContext.Provider>
}

export default ActivePickaxeProvider
