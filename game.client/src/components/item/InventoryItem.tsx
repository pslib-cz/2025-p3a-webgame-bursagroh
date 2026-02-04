import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'

type InventoryItemProps = {
    items: InventoryItemType[]
}

const InventoryItem: React.FC<InventoryItemProps> = ({ items }) => {
    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", items[0].inventoryItemId.toString())
    }

    return (
        <div draggable onDragStart={handleOnDragStart}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(items[0].itemInstance.item.itemId)} />
            </svg>
            <span>{items[0].itemInstance.durability}</span>
            {","}
            <span>{items[0].itemInstance.item.weight}</span>
            {","}
            <span>{items.length}x</span>
        </div>
    )
}

export default InventoryItem