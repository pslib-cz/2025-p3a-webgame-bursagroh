import React from 'react'
import type { InventoryItem } from '../../types/api/models/player'
import Asset from '../SVG/Asset'
import { itemIdToAssetType } from '../../utils/item'

type HandItemProps = {
    item: InventoryItem
}

const HandItem: React.FC<HandItemProps> = ({ item }) => {
    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", item.inventoryItemId.toString())
    }

    return (
        <div draggable onDragStart={handleOnDragStart}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(item.itemInstance.item.itemId)} />
            </svg>
            <span>{item.itemInstance.durability}</span>
        </div>
    )
}

export default HandItem