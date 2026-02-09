import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { equipItemMutation } from '../../api/player'
import { useMutation } from '@tanstack/react-query'
import useNotification from '../../hooks/useNotification'
import Item from './Item'

type InventoryItemProps = {
    items: InventoryItemType[]
}

const InventoryItem: React.FC<InventoryItemProps> = ({ items }) => {
    const { genericError } = useNotification()

    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { mutateAsync: equipItemAsync } = useMutation(equipItemMutation(playerId, genericError))

    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", `inv_${items[0].inventoryItemId.toString()}`)
    }

    const handleClick = () => {
        equipItemAsync(items[0].inventoryItemId)
    }

    return (
        <Item tooltipHeading={items[0].itemInstance.item.name}
            tooltipText={items[0].itemInstance.item.description}
            assetType={itemIdToAssetType(items[0].itemInstance.item.itemId)}
            amount={items.length}
            durability={items[0].itemInstance.durability}
            weight={items[0].itemInstance.item.weight}
            draggable
            onDragStart={handleOnDragStart}
            onClick={handleClick} />
    )
}

export default InventoryItem