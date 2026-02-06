export type IngredienceType = "Meat" | "Salad" | "BunUp" | "BunDown" | "Tomato" | "Sauce" | "Bacon" | "Cheese"

export type Recipe = {
    recipeId: number
    name: string
    ingrediences: Array<Ingredience>
}

export type Ingredience = {
    order: number
    ingredienceType: IngredienceType
}

export type RecipeResult = {
    duration: string
    money: number
}

export type LeaderboardEntry = {
    recipeId: number,
    duration: number
}
