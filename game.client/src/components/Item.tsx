import React from 'react'
import type { InventoryItem } from '../types/api/models/player'

type ItemProps = {
    item: InventoryItem
}

const Item: React.FC<ItemProps> = ({ item }) => {
    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", item.inventoryItemId.toString())
    }
    
    return (
        <div draggable onDragStart={handleOnDragStart}>
            Item: {item.itemInstance.item.name}
        </div>
    )
}

export default Item