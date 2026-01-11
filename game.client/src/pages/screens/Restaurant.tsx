import React from "react"
import { useMutation, useQuery } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { endRecipeMutation, getRandomRecipeMutation, getRecipesQuery, startRecipeMutation } from "../../api/recipe"
import type { IngredienceType, Recipe } from "../../types/api/models/recipe"
import { useNavigate } from "react-router"

const RestaurantScreen = () => {
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const {mutateAsync: getRandomRecipeAsync} = useMutation(getRandomRecipeMutation())
    const {mutateAsync: startRecipeAsync} = useMutation(startRecipeMutation(playerId))
    const {mutateAsync: endRecipeAsync} = useMutation(endRecipeMutation(playerId))

    const [isMaking, setIsMaking] = React.useState(false)
    const [currentBurgerStack, setCurrentBurgerStack] = React.useState<Array<IngredienceType>>([])
    const [currentBurger, setCurrentBurger] = React.useState<Recipe>()

    const recipes = useQuery(getRecipesQuery())

    const handleClose = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    const handleStart = async () => {
        const recipe = await getRandomRecipeAsync()

        await startRecipeAsync(recipe.recipeId)

        setCurrentBurger(recipe)
        setIsMaking(true)
    }

    const handleStop = async () => {
        await endRecipeAsync({recipeId: currentBurger!.recipeId, playerAssembly: currentBurgerStack})

        setCurrentBurgerStack([])
        setCurrentBurger(undefined)
        setIsMaking(false)
    }

    const addIngredience = (ingredienceType: IngredienceType) => {
        setCurrentBurgerStack(prev => [...prev, ingredienceType])
    }

    if (recipes.isLoading) {
        return <div>Loading...</div>
    }

    if (recipes.isError) {
        return <div>Error loading recipes</div>
    }

    if (recipes.isSuccess) {
        if (isMaking) {
            return (
                <>
                    <div>Restaurant</div>
                    <p>Make this: {currentBurger?.name}</p>
                    <button onClick={handleStop}>finish</button>
                    <button onClick={() => addIngredience("BunDown")}>Add Bun Down</button>
                    <button onClick={() => addIngredience("Meat")}>Add Meat</button>
                    <button onClick={() => addIngredience("Cheese")}>Add Cheese</button>
                    <button onClick={() => addIngredience("Salad")}>Add Salad</button>
                    <button onClick={() => addIngredience("Tomato")}>Add Tomato</button>
                    <button onClick={() => addIngredience("Bacon")}>Add Bacon</button>
                    <button onClick={() => addIngredience("Sauce")}>Add Sauce</button>
                    <button onClick={() => addIngredience("BunUp")}>Add Bun Up</button>
                    <button onClick={() => setCurrentBurgerStack([])}>Clear</button>
                    {currentBurgerStack.map((ingredience, index) => (
                        <div key={index}>
                            {index + 1}. {ingredience}
                        </div>
                    ))}
                </>
            )
        }

        return (
            <>
                <div>Restaurant</div>
                <button onClick={handleStart}>make burger</button>
                <button onClick={handleClose}>close</button>
                {recipes.data.map((recipe) => (
                    <div key={recipe.recipeId}>
                        <p>{recipe.name}</p>
                        {recipe.ingrediences.sort((a, b) => a.order - b.order).map((ingredience, index) => (
                            <div key={index}>
                                {ingredience.order}. {ingredience.ingredienceType}
                            </div>
                        ))}
                    </div>
                ))}
            </>
        )
    }
}

export default RestaurantScreen
