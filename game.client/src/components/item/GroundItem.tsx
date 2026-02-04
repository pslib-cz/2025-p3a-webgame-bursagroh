import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { FloorItemInstance } from '../../types/api/models/building'
import { useMutation } from '@tanstack/react-query'
import { pickItemMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'

type GroundItemProps = {
    items: {floorItemId: number, item: FloorItemInstance}[]
}

const GroundItem: React.FC<GroundItemProps> = ({ items }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const {mutateAsync: pickItemAsync} = useMutation(pickItemMutation(playerId))

    const handleClick = () => {
        pickItemAsync(items[0].floorItemId)
    }

    return (
        <div onClick={handleClick}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(items[0].item.item.itemId)} />
            </svg>
            <span>{items[0].item.durability}</span>
            {","}
            <span>{items[0].item.item.weight}</span>
            {","}
            <span>{items.length}x</span>
        </div>
    )
}

export default GroundItem