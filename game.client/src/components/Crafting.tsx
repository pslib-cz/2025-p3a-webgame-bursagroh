import React from 'react'
import CraftingItem from './item/CraftingItem'
import type { Blueprint } from '../types/api/models/blueprint'
import Asset from './SVG/Asset'
import { itemIdToAssetType } from '../utils/item'
import styles from './crafting.module.css'
import Tooltip from './Tooltip'
import Text from './Text'

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
                        <Tooltip heading="Material" text={crafting.item.name}>
                            <svg className={styles.asset} viewBox="0 0 1 1">
                                <Asset assetType={itemIdToAssetType(crafting.item.itemId)} width={1} height={1} />
                            </svg>
                        </Tooltip>
                        <Text size="h4" className={styles.craftingItemText}>{crafting.amount}x</Text>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default Crafting