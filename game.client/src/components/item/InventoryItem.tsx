import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'

type InventoryItemProps = {
    count: number
    item: InventoryItemType
}

const InventoryItem: React.FC<InventoryItemProps> = ({ item, count }) => {
    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", item.inventoryItemId.toString())
    }

    return (
        <div draggable onDragStart={handleOnDragStart}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(item.itemInstance.item.itemId)} />
            </svg>
            <span>{item.itemInstance.durability}</span>
            {","}
            <span>{item.itemInstance.item.weight}</span>
            {","}
            <span>{count}x</span>
        </div>
    )
}

export default InventoryItem