import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'
import ArrowRightIcon from '../../assets/icons/ArrowRightIcon'
import ArrowRightDoubleIcon from '../../assets/icons/ArrowRightDoubleIcon'
import { moveBankItemMutation } from '../../api/bank'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'

type BankInventoryItemProps = {
    items: InventoryItemType[]
}

const BankInventoryItem: React.FC<BankInventoryItemProps> = ({ items }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { mutateAsync: moveBankItemAsync } = useMutation(moveBankItemMutation(playerId))

    const handleSingleMove = () => {
        moveBankItemAsync({inventoryItemIds: [items[0].inventoryItemId]})
    }

    const handleMultipleMove = () => {
        moveBankItemAsync({inventoryItemIds: items.map(item => item.inventoryItemId)})
    }

    return (
        <div>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(items[0].itemInstance.item.itemId)} />
            </svg>
            <span>{items[0].itemInstance.durability}</span>
            {","}
            <span>{items[0].itemInstance.item.weight}</span>
            {","}
            <span>{items.length}x</span>
            <ArrowRightIcon width={24} height={24} onClick={handleSingleMove} />
            <ArrowRightDoubleIcon width={24} height={24} onClick={handleMultipleMove} />
        </div>
    )
}

export default BankInventoryItem