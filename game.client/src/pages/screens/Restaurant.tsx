import React, { type JSX } from "react"
import { useMutation } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { endRecipeMutation, getRandomRecipeMutation, startRecipeMutation } from "../../api/recipe"
import type { IngredienceType, Recipe } from "../../types/api/models/recipe"
import { useNavigate } from "react-router"
import { RecipesContext } from "../../providers/game/RecipesProvider"
import Burger from "../../components/Burger"
import useBlur from "../../hooks/useBlur"

const RestaurantScreen = () => {
    useBlur(true)
    
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const {mutateAsync: getRandomRecipeAsync} = useMutation(getRandomRecipeMutation())
    const {mutateAsync: startRecipeAsync} = useMutation(startRecipeMutation(playerId))
    const {mutateAsync: endRecipeAsync} = useMutation(endRecipeMutation(playerId))

    const [isMaking, setIsMaking] = React.useState(false)
    const [currentBurgerStack, setCurrentBurgerStack] = React.useState<Array<IngredienceType>>([])
    const [currentBurger, setCurrentBurger] = React.useState<Recipe>()

    const recipes = React.useContext(RecipesContext)!.recipes!

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

    let cookingSection: JSX.Element | null = null
    if (isMaking) {
        cookingSection = (
            <div>
                <span>{currentBurger?.name}</span>
                <Burger burger={{recipeId: currentBurger?.recipeId ?? -1, name: "", ingrediences: currentBurgerStack.map((ingredienceType, index) => ({order: index, ingredienceType}))}} />
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
            </div>
        )
    } else {
        cookingSection = (
            <div>
                <button onClick={handleStart}>make burger</button>
            </div>
        )
    }

    return (
        <div>
            <div>
                <span>Recipes</span>
                <span>Leaderboard</span>
                {recipes.map((recipe) => (
                    <div key={recipe.recipeId}>
                        <Burger burger={recipe} />
                        <div>
                            <span>1. Player1 - 10</span>
                            <span>2. Anonym - 8</span>
                            <span>3. Player3 - 5</span>
                        </div>
                    </div>
                ))}
            </div>
            <div>
                <span>Cooking</span>
                <button onClick={handleClose}>close</button>
                {cookingSection}
            </div>
        </div>
    )
}

export default RestaurantScreen
