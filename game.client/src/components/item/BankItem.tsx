import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'
import { useMutation } from '@tanstack/react-query'
import { moveBankItemMutation } from '../../api/bank'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import useNotification from '../../hooks/useNotification'
import useLock from '../../hooks/useLock'
import Item from './Item'

type BankItemProps = {
    items: InventoryItemType[]
}

const BankItem: React.FC<BankItemProps> = ({ items }) => {
    const {genericError} = useNotification()
    const handleLock = useLock()

    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { mutateAsync: moveBankItemAsync } = useMutation(moveBankItemMutation(playerId, genericError))

    const handleSingleMove = async () => {
        await handleLock(async () => {
            await moveBankItemAsync({inventoryItemIds: [items[0].inventoryItemId]})
        })
    }

    const handleMultipleMove = async () => {
        await handleLock(async () => {
            await moveBankItemAsync({inventoryItemIds: items.map(item => item.inventoryItemId)})
        })
    }

    return (
        <Item tooltipHeading={items[0].itemInstance.item.name}
            tooltipText={items[0].itemInstance.item.description}
            assetType={itemIdToAssetType(items[0].itemInstance.item.itemId)}
            weight={items[0].itemInstance.item.weight}
            durability={items[0].itemInstance.durability}
            amount={items.length}
            onSingleMoveLeft={handleSingleMove}
            onMultipleMoveLeft={handleMultipleMove}
             />
    )
}

export default BankItem