import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { InventoryItem as InventoryItemType } from '../../types/api/models/player'
import ArrowLeftIcon from '../../icons/ArrowLeftIcon'
import ArrowLeftDoubleIcon from '../../icons/ArrowLeftDoubleIcon'
import { useMutation } from '@tanstack/react-query'
import { moveBankItemMutation } from '../../api/bank'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import styles from './bankItem.module.css'
import WeightIcon from '../../icons/WeightIcon'
import ConditionalDisplay from '../wrappers/ConditionalDisplay'
import Tooltip from '../Tooltip'
import useNotification from '../../hooks/useNotification'
import useLock from '../../hooks/useLock'

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
        <Tooltip heading={items[0].itemInstance.item.name} text={items[0].itemInstance.item.description}>
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
                <ArrowLeftIcon className={styles.transferSingle} width={32} height={32} onClick={handleSingleMove} />
                <ArrowLeftDoubleIcon className={styles.transferMulti} width={32} height={32} onClick={handleMultipleMove} />
            </div>
        </Tooltip>
    )
}

export default BankItem