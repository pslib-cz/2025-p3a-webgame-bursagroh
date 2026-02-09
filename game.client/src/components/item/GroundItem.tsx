import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import type { FloorItemInstance } from '../../types/api/models/building'
import { useMutation } from '@tanstack/react-query'
import { pickItemMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import useNotification from '../../hooks/useNotification'
import useLock from '../../hooks/useLock'
import Item from './Item'

type GroundItemProps = {
    items: {floorItemId: number, item: FloorItemInstance}[]
}

const GroundItem: React.FC<GroundItemProps> = ({ items }) => {
    const {genericError} = useNotification()
    const handleLock = useLock()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const {mutateAsync: pickItemAsync} = useMutation(pickItemMutation(playerId, genericError))

    const handleClick = async () => {
        await handleLock(async () => {
            await pickItemAsync(items[0].floorItemId)
        })
    }

    return (
        <Item tooltipHeading={items[0].item.item.name}
            tooltipText={items[0].item.item.description}
            assetType={itemIdToAssetType(items[0].item.item.itemId)}
            durability={items[0].item.durability}
            weight={items[0].item.item.weight}
            amount={items.length}
            onClick={handleClick} />
    )
}

export default GroundItem