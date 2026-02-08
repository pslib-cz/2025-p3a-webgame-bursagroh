import React from "react"

type IsBluredContextType = {
    isBlured: boolean
    setIsBlured: React.Dispatch<React.SetStateAction<boolean>>
}

// eslint-disable-next-line react-refresh/only-export-components
export const IsBluredContext = React.createContext<IsBluredContextType | null>(null)

const IsBluredProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [isBlured, setIsBlured] = React.useState(false)

    return <IsBluredContext.Provider value={{ isBlured, setIsBlured }}>{children}</IsBluredContext.Provider>
}

export default IsBluredProvider   