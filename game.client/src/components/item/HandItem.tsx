import React from 'react'
import type { InventoryItem } from '../../types/api/models/player'
import { itemIdToAssetType } from '../../utils/item'
import useUse from '../../hooks/useUse'
import { useMutation } from '@tanstack/react-query'
import { equipItemMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import useNotification from '../../hooks/useNotification'
import Item from './Item'

type HandItemProps = {
    item: InventoryItem
}

const HandItem: React.FC<HandItemProps> = ({ item }) => {
    const handleUse = useUse()
    const {genericError} = useNotification()

    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const {mutateAsync: equipItemAsync} = useMutation(equipItemMutation(playerId, genericError))

    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", `hand_${item.inventoryItemId.toString()}`)
    }

    const handleClick = async () => {
        if (item.itemInstance.item.itemType === "Potion") {
            await handleUse()
            return
        }

        await equipItemAsync(null)
    }

    return (
        <Item tooltipHeading={item.itemInstance.item.name}
            tooltipText={item.itemInstance.item.description}
            assetType={itemIdToAssetType(item.itemInstance.item.itemId)}
            durability={item.itemInstance.durability}
            draggable
            onDragStart={handleOnDragStart}
            onClick={handleClick} />
    )
}

export default HandItem