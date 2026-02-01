import React from "react"

type IsOpenInventoryContextType = {
    isOpen: boolean
    setIsOpen: React.Dispatch<React.SetStateAction<boolean>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const IsOpenInventoryContext = React.createContext<IsOpenInventoryContextType | null>(null)

const IsOpenInventoryProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [isOpen, setIsOpen] = React.useState(false)

    return <IsOpenInventoryContext.Provider value={{ isOpen, setIsOpen }}>{children}</IsOpenInventoryContext.Provider>
}

export default IsOpenInventoryProvider   