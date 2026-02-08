import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getBlueprintsQuery } from "../../api/blueprint"
import type { Blueprint } from "../../types/api/models/blueprint"

type BlueprintContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    blueprints: Blueprint[] | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const BlueprintContext = React.createContext<BlueprintContextType | null>(null)

const BlueprintProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const {data: blueprints, isError, isPending, isSuccess} = useQuery(getBlueprintsQuery())

    return <BlueprintContext.Provider value={{ blueprints, isError, isPending, isSuccess }}>{children}</BlueprintContext.Provider>
}

export default BlueprintProvider   