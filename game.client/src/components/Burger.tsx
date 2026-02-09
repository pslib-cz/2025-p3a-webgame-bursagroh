import React from 'react'
import type { Recipe } from '../types/api/models/recipe'
import Asset from './SVG/Asset'
import styles from './burger.module.css'
import { ingredienceToAssetType } from '../utils/recipe'
import { INGREDIENCE_PIXEL_HEIGHT } from '../constants/burger'
import Text from './Text'
import { readCSSProperty } from '../utils/window'

type BurgerProps = {
    burger: Recipe
}

const Burger: React.FC<BurgerProps> = ({ burger }) => {
    const [itemSize, setItemSize] = React.useState(INGREDIENCE_PIXEL_HEIGHT)

    React.useEffect(() => {
        const update = () => {
            const itemSize = readCSSProperty('--item-size')
            const fontSize = readCSSProperty('font-size')

            setItemSize(itemSize * fontSize / 8)
        }

        update()

        window.addEventListener('resize', update)

        return () => window.removeEventListener('resize', update)
    }, [])

    const height = burger.ingrediences.length * itemSize + 3 * itemSize

    return (
        <div className={styles.burgerContainer}>
            <svg width={itemSize * 8} height={height} viewBox={`0 0 ${itemSize * 8} ${height}`} xmlns="http://www.w3.org/2000/svg">
                {burger.ingrediences.sort((a, b) => b.order - a.order).map((ingredience, index) => (
                    <Asset key={index} x={0} y={height - itemSize * 8 - itemSize * index} width={itemSize * 8} height={itemSize * 8} assetType={ingredienceToAssetType(ingredience.ingredienceType)} />
                ))}
            </svg>
            <Text size="h4" className={styles.burgerName}>{burger.name}</Text>
        </div>
    )
}

export default Burger