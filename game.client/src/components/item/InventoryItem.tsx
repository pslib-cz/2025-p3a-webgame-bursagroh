import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'
import ConditionalDisplay from '../wrappers/ConditionalDisplay'
import WeightIcon from '../../assets/icons/WeightIcon'
import styles from './inventoryItem.module.css'

type InventoryItemProps = {
    items: InventoryItemType[]
}

const InventoryItem: React.FC<InventoryItemProps> = ({ items }) => {
    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", items[0].inventoryItemId.toString())
    }

    return (
        <div className={styles.container} draggable onDragStart={handleOnDragStart}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(items[0].itemInstance.item.itemId)} width={128} height={128} />
            </svg>
            <ConditionalDisplay condition={items[0].itemInstance.durability !== 0}>
                <span className={styles.durability}>{items[0].itemInstance.durability}</span>
            </ConditionalDisplay>
            <div className={styles.weight}>
                <WeightIcon className={styles.weightIcon} width={24} height={24} />
                <span className={styles.weightText}>{items[0].itemInstance.item.weight}</span>
            </div>
            <span className={styles.amount}>{items.length}x</span>
        </div>
    )
}

export default InventoryItem