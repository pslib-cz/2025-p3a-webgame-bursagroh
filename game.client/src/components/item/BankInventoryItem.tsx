import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'
import ArrowRightIcon from '../../assets/icons/ArrowRightIcon'
import ArrowRightDoubleIcon from '../../assets/icons/ArrowRightDoubleIcon'
import { moveBankItemMutation } from '../../api/bank'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import styles from './bankInventoryItem.module.css'
import ConditionalDisplay from '../wrappers/ConditionalDisplay'
import WeightIcon from '../../assets/icons/WeightIcon'

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
        <div className={styles.container}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(items[0].itemInstance.item.itemId)} width={128} height={128} />
            </svg>
            <ConditionalDisplay condition={items[0].itemInstance.durability !== 0}>
                <span className={styles.durability}>{items[0].itemInstance.durability}</span>
            </ConditionalDisplay>
            <div className={styles.weight}>
                <WeightIcon className={styles.weightIcon} width={24} height={24} />
                <span className={styles.weightText}>{items[0].itemInstance.item.weight}</span>
            </div>
            <span className={styles.amount}>{items.length}x</span>
            <ArrowRightIcon className={styles.transferSingle} width={32} height={32} onClick={handleSingleMove} />
            <ArrowRightDoubleIcon className={styles.transferMulti} width={32} height={32} onClick={handleMultipleMove} />
        </div>
    )
}

export default BankInventoryItem