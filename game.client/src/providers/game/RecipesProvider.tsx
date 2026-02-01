import { useQuery } from "@tanstack/react-query"
import React from "react"
import type { Recipe } from "../../types/api/models/recipe"
import { getRecipesQuery } from "../../api/recipe"

type RecipesContextType = {
    isError: boolean
    isPending: boolean
    isSuccess: boolean
    recipes: Recipe[] | undefined
}

// eslint-disable-next-line react-refresh/only-export-components
export const RecipesContext = React.createContext<RecipesContextType | null>(null)

const RecipesProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const {data: recipes, isError, isPending, isSuccess} = useQuery(getRecipesQuery())

    return <RecipesContext.Provider value={{ recipes, isError, isPending, isSuccess }}>{children}</RecipesContext.Provider>
}

export default RecipesProvider   