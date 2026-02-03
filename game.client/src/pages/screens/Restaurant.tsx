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
import styles from './restaurant.module.css'
import Asset from "../../components/SVG/Asset"

const RestaurantScreen = () => {
    useBlur(true)

    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const { mutateAsync: getRandomRecipeAsync } = useMutation(getRandomRecipeMutation())
    const { mutateAsync: startRecipeAsync } = useMutation(startRecipeMutation(playerId))
    const { mutateAsync: endRecipeAsync } = useMutation(endRecipeMutation(playerId))

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
        await endRecipeAsync({ recipeId: currentBurger?.recipeId ?? -1, playerAssembly: currentBurgerStack })

        setCurrentBurgerStack([])
        setCurrentBurger(undefined)
        setIsMaking(false)
    }

    const addIngredience = (ingredienceType: IngredienceType) => {
        setCurrentBurgerStack(prev => [ingredienceType, ...prev])
    }

    let cookingSection: JSX.Element | null = null
    if (isMaking) {
        cookingSection = (
            <div className={styles.cookingContainer}>
                <span className={styles.burgerName}>{currentBurger?.name}</span>
                <Burger burger={{ recipeId: currentBurger?.recipeId ?? -1, name: "", ingrediences: currentBurgerStack.map((ingredienceType, index) => ({ order: index, ingredienceType })) }} />
                <div className={styles.ingredienceButtons}>
                    <svg onClick={() => addIngredience("BunDown")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="bun_down" />
                    </svg>
                    <svg onClick={() => addIngredience("Meat")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="meat" />
                    </svg>
                    <svg onClick={() => addIngredience("Cheese")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="cheese" />
                    </svg>
                    <svg onClick={() => addIngredience("Salad")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="salad" />
                    </svg>
                    <svg onClick={() => addIngredience("Tomato")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="tomato" />
                    </svg>
                    <svg onClick={() => addIngredience("Bacon")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="bacon" />
                    </svg>
                    <svg onClick={() => addIngredience("Sauce")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="sauce" />
                    </svg>
                    <svg onClick={() => addIngredience("BunUp")} width={128} height={128} viewBox="0 0 32 32" xmlns="http://www.w3.org/2000/svg">
                        <Asset x={0} y={0} width={32} height={32} assetType="bun_up" />
                    </svg>
                </div>
                <div>
                    <button onClick={handleStop}>Done</button>
                    <button onClick={() => setCurrentBurgerStack([])}>Clear</button>
                </div>
            </div>
        )
    } else {
        cookingSection = (
            <div className={styles.cookingContainer}>
                <button onClick={handleStart}>Start</button>
            </div>
        )
    }

    return (
        <div className={styles.container}>
            <div className={styles.recipesLeaderboardContainer}>
                <span className={styles.heading}>Recipes</span>
                <span className={styles.heading}>Leaderboard</span>
                <div className={styles.recipesContainer}>
                    {recipes.map((recipe) => (
                        <div key={recipe.recipeId} className={styles.burgerContainer}>
                            <Burger burger={recipe} />
                            <div className={styles.ingrediencesList}>
                                {recipe.ingrediences.sort((a, b) => a.order - b.order).map((ingredience) => (
                                    <span key={ingredience.order} className={styles.ingredienceText}>{ingredience.ingredienceType}</span>
                                ))}
                            </div>
                        </div>
                    ))}
                </div>
                <div className={styles.leaderboardContainer}>
                    {recipes.map((recipe) => (
                        <div key={recipe.recipeId} className={styles.burgerContainer}>
                            <Burger burger={recipe} />
                            <div>
                                <span>1. Player1 - 10</span>
                                <span>2. Anonym - 8</span>
                                <span>3. Player3 - 5</span>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
            <div className={styles.cookingContainer}>
                <span className={styles.heading}>Cooking</span>
                <button className={styles.close} onClick={handleClose}>close</button>
                {cookingSection}
            </div>
        </div>
    )
}

export default RestaurantScreen
