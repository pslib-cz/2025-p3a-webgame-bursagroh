import React from 'react'
import CraftingItem from './item/CraftingItem'
import type { Blueprint } from '../types/api/models/blueprint'
import Asset from './SVG/Asset'
import { itemIdToAssetType } from '../utils/item'
import styles from './crafting.module.css'

type CraftingProps = {
    blueprint: Blueprint
}

const Crafting: React.FC<CraftingProps> = ({ blueprint }) => {
    return (
        <div className={styles.container}>
            <CraftingItem blueprint={blueprint} />
            <div className={styles.itemContainer}>
                {blueprint.craftings.map((crafting) => (
                    <div className={styles.craftingItemContainer} key={crafting.item.itemId}>
                        <svg width="32" height="32" viewBox="0 0 128 128">
                            <Asset assetType={itemIdToAssetType(crafting.item.itemId)} width={128} height={128} />
                        </svg>
                        <span className={styles.craftingItemText}>{crafting.amount}x</span>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default Crafting