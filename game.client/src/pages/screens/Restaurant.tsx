import React, { type JSX } from "react"
import { useMutation } from "@tanstack/react-query"
import { PlayerIdContext } from "../../providers/global/PlayerIdProvider"
import { endRecipeMutation, getRandomRecipeMutation, startRecipeMutation } from "../../api/recipe"
import type { IngredienceType, Recipe } from "../../types/api/models/recipe"
import RecipesProvider, { RecipesContext } from "../../providers/game/RecipesProvider"
import Burger from "../../components/Burger"
import useBlur from "../../hooks/useBlur"
import styles from './restaurant.module.css'
import Asset from "../../components/SVG/Asset"
import CloseIcon from "../../icons/CloseIcon"
import Button from "../../components/Button"
import LeaderboardProvider, { LeaderboardContext } from "../../providers/game/LeaderboardProvider"
import useNotification from "../../hooks/useNotification"
import useKeyboard from "../../hooks/useKeyboard"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import type { TLoadingWrapperContextState } from '../../types/context'
import useLock from "../../hooks/useLock"
import Tooltip from "../../components/Tooltip"
import useLink from "../../hooks/useLink"
import Text from "../../components/Text"

const RestaurantScreenWithContext = () => {
    useBlur(true)

    const moveToPage = useLink()
    const { genericError } = useNotification()
    const handleLock = useLock()

    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const recipes = React.useContext(RecipesContext)!.recipes!
    const leaderboard = React.useContext(LeaderboardContext)!.leaderboard!

    const { mutateAsync: getRandomRecipeAsync } = useMutation(getRandomRecipeMutation(genericError))
    const { mutateAsync: startRecipeAsync } = useMutation(startRecipeMutation(playerId, genericError))
    const { mutateAsync: endRecipeAsync } = useMutation(endRecipeMutation(playerId, genericError))

    const [isMaking, setIsMaking] = React.useState(false)
    const [currentBurgerStack, setCurrentBurgerStack] = React.useState<Array<IngredienceType>>([])
    const [currentBurger, setCurrentBurger] = React.useState<Recipe>()

    const handleEscape = async () => {
        await moveToPage("city", true)
    }

    const handleStart = async () => {
        await handleLock(async () => {
            const recipe = await getRandomRecipeAsync()

            await startRecipeAsync(recipe.recipeId)

            setCurrentBurger(recipe)
            setIsMaking(true)
        })
    }

    const handleStop = async () => {
        await handleLock(async () => {
            await endRecipeAsync({ recipeId: currentBurger?.recipeId ?? -1, playerAssembly: currentBurgerStack })

            setCurrentBurgerStack([])
            setCurrentBurger(undefined)
            setIsMaking(false)
        })
    }

    const addIngredience = (ingredienceType: IngredienceType) => {
        setCurrentBurgerStack(prev => [ingredienceType, ...prev])
    }

    useKeyboard("Escape", handleEscape)

    let cookingSection: JSX.Element | null = null
    if (isMaking) {
        cookingSection = (
            <div className={styles.innerCookingContainer}>
                <Text size="h2" className={styles.burgerName}>{currentBurger?.name}</Text>
                <Burger burger={{ recipeId: currentBurger?.recipeId ?? -1, name: "", ingrediences: currentBurgerStack.map((ingredienceType, index) => ({ order: index, ingredienceType })) }} />
                <div className={styles.ingredienceButtons}>
                    <Tooltip heading="Ingredience" text="Bun down">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("BunDown")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="bun_down" />
                        </svg>
                    </Tooltip>
                    <Tooltip heading="Ingredience" text="Meat">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("Meat")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="meat" />
                        </svg>
                    </Tooltip>
                    <Tooltip heading="Ingredience" text="Cheese">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("Cheese")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="cheese" />
                        </svg>
                    </Tooltip>
                    <Tooltip heading="Ingredience" text="Salad">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("Salad")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="salad" />
                        </svg>
                    </Tooltip>
                    <Tooltip heading="Ingredience" text="Tomato">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("Tomato")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="tomato" />
                        </svg>
                    </Tooltip>
                    <Tooltip heading="Ingredience" text="Bacon">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("Bacon")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="bacon" />
                        </svg>
                    </Tooltip>
                    <Tooltip heading="Ingredience" text="Sauce">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("Sauce")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="sauce" />
                        </svg>
                    </Tooltip>
                    <Tooltip heading="Ingredience" text="Bun up">
                        <svg className={`${styles.button} ${styles.asset}`} onClick={() => addIngredience("BunUp")} viewBox="0 0 1 1" xmlns="http://www.w3.org/2000/svg">
                            <Asset x={0} y={0} width={1} height={1} assetType="bun_up" />
                        </svg>
                    </Tooltip>
                </div>
                <div className={styles.buttonContainer}>
                    <Button onClick={handleStop}>Done</Button>
                    <Button onClick={() => setCurrentBurgerStack([])}>Clear</Button>
                </div>
            </div>
        )
    } else {
        cookingSection = (
            <div className={styles.innerCookingContainer}>
                <div className={styles.startButton}>
                    <Button onClick={handleStart}>Start</Button>
                </div>
            </div>
        )
    }

    return (
        <div className={styles.container}>
            <div className={styles.recipesLeaderboardContainer}>
                <Text size="h3" className={styles.heading}>Recipes</Text>
                <Text size="h3" className={styles.heading}>Leaderboard</Text>
                <div className={styles.recipesContainer}>
                    {recipes.map((recipe) => (
                        <div key={recipe.recipeId} className={styles.burgerContainer}>
                            <Burger burger={recipe} />
                            <div className={styles.ingrediencesList}>
                                {recipe.ingrediences.sort((a, b) => a.order - b.order).map((ingredience) => (
                                    <Text key={ingredience.order} size="h4">{ingredience.ingredienceType}</Text>
                                ))}
                            </div>
                        </div>
                    ))}
                </div>
                <div className={styles.leaderboardContainer}>
                    {recipes.map((recipe) => (
                        <div key={recipe.recipeId} className={styles.burgerContainer}>
                            <Burger burger={recipe} />
                            <div className={styles.leaderboard}>
                                {Array.from({ length: 3 }, (_, i) => i + 1).map((position) => (
                                    <Text key={position} size="h4" className={styles.leaderboardText}>{position}. {leaderboard.filter(entry => entry.recipeId === recipe.recipeId).sort((a, b) => a.duration - b.duration)[position - 1] ? Math.round(leaderboard.filter(entry => entry.recipeId === recipe.recipeId).sort((a, b) => a.duration - b.duration)[position - 1].duration * 1000) + "ms" : "N/A"}</Text>
                                ))}
                            </div>
                        </div>
                    ))}
                </div>
            </div>
            <div className={styles.cookingContainer}>
                <Text size="h3" className={styles.heading}>Cooking</Text>
                <CloseIcon className={styles.close} onClick={handleEscape} />
                {cookingSection}
            </div>
        </div>
    )
}

const RestaurantScreen = () => {
    return (
        <ProviderGroupLoadingWrapper providers={[RecipesProvider, LeaderboardProvider]} contextsToLoad={[RecipesContext, LeaderboardContext] as Array<React.Context<TLoadingWrapperContextState>>}>
            <RestaurantScreenWithContext />
        </ProviderGroupLoadingWrapper>
    )
}

export default RestaurantScreen
