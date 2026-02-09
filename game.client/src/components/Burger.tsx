import React from 'react'
import type { Recipe } from '../types/api/models/recipe'
import Asset from './SVG/Asset'
import styles from './burger.module.css'
import { ingredienceToAssetType } from '../utils/recipe'
import { INGREDIENCE_PIXEL_HEIGHT } from '../constants/burger'

type BurgerProps = {
    burger: Recipe
}

const Burger: React.FC<BurgerProps> = ({ burger }) => {
    const height = burger.ingrediences.length * INGREDIENCE_PIXEL_HEIGHT + 3 * INGREDIENCE_PIXEL_HEIGHT

    return (
        <div className={styles.burgerContainer}>
            <svg width={INGREDIENCE_PIXEL_HEIGHT * 8} height={height} viewBox={`0 0 ${INGREDIENCE_PIXEL_HEIGHT * 8} ${height}`} xmlns="http://www.w3.org/2000/svg">
                {burger.ingrediences.sort((a, b) => b.order - a.order).map((ingredience, index) => (
                    <Asset key={index} x={0} y={height - INGREDIENCE_PIXEL_HEIGHT * 8 - INGREDIENCE_PIXEL_HEIGHT * index} width={INGREDIENCE_PIXEL_HEIGHT * 8} height={INGREDIENCE_PIXEL_HEIGHT * 8} assetType={ingredienceToAssetType(ingredience.ingredienceType)} />
                ))}
            </svg>
            <span className={styles.burgerName}>{burger.name}</span>
        </div>
    )
}

export default Burger