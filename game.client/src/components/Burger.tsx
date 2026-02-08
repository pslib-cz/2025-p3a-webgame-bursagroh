import React from 'react'
import type { IngredienceType, Recipe } from '../types/api/models/recipe'
import Asset from './SVG/Asset'
import type { AssetType } from '../types/asset'
import styles from './burger.module.css'

type BurgerProps = {
    burger: Recipe
}

const mapIngredienceToAssetType = (ingredienceType: IngredienceType): AssetType => {
    switch (ingredienceType) {
        case 'Meat':
            return "meat"
        case 'Salad':
            return "salad"
        case 'BunUp':
            return "bun_up"
        case 'BunDown':
            return "bun_down"
        case 'Tomato':
            return "tomato"
        case 'Sauce':
            return "sauce"
        case 'Bacon':
            return "bacon"
        case 'Cheese':
            return "cheese"
    }
}

const Burger: React.FC<BurgerProps> = ({ burger }) => {
    const ingrediencePixelHeight = 16
    const height = burger.ingrediences.length * ingrediencePixelHeight + 3 * ingrediencePixelHeight

    return (
        <div className={styles.burgerContainer}>
            <svg width={ingrediencePixelHeight * 8} height={height} viewBox={`0 0 ${ingrediencePixelHeight * 8} ${height}`} xmlns="http://www.w3.org/2000/svg">
                {burger.ingrediences.sort((a, b) => b.order - a.order).map((ingredience, index) => (
                    <Asset key={index} x={0} y={height - ingrediencePixelHeight * 8 - ingrediencePixelHeight * index} width={ingrediencePixelHeight * 8} height={ingrediencePixelHeight * 8} assetType={mapIngredienceToAssetType(ingredience.ingredienceType)} />
                ))}
            </svg>
            <span className={styles.burgerName}>{burger.name}</span>
        </div>
    )
}

export default Burger