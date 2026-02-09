import OpenIcon from '../icons/OpenIcon'
import HeartIcon from '../icons/HeartIcon'
import MoneyIcon from '../icons/MoneyIcon'
import WeightIcon from '../icons/WeightIcon'
import styles from './playerUI.module.css'
import React from 'react'
import { calcInventoryWeight } from '../utils/inventory'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { InventoryContext } from '../providers/game/InventoryProvider'
import { equipItemMutation } from '../api/player'
import { useMutation } from '@tanstack/react-query'
import HandItem from './item/HandItem'
import { IsOpenInventoryContext } from '../providers/game/IsOpenInventoryProvider'
import ArrayDisplay from './wrappers/ArrayDisplay'
import useNotification from '../hooks/useNotification'
import Text from './Text'

const PlayerUI = () => {
    const {genericError} = useNotification()
    
    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const { setIsOpen } = React.useContext(IsOpenInventoryContext)!

    const {mutateAsync: equipItemAsync} = useMutation(equipItemMutation(player.playerId, genericError))

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault()
        const data = event.dataTransfer.getData("text/plain")
        if (data.startsWith("inv_")) {
            const inventoryItemId = Number(data.substring(4))
            equipItemAsync(inventoryItemId)
        }
    }

    const handleOpenInventory = () => {
        setIsOpen(prev => !prev)
    }

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <Text size="h3">Player</Text>
                <OpenIcon className={styles.open} width={24} height={24} onClick={handleOpenInventory} />
            </div>
            <div className={styles.statContainer}>
                <div className={styles.stat}>
                    <HeartIcon width={24} height={24} />
                    <Text size="h4">{player.health} / {player.maxHealth}</Text>
                </div>
                <div className={styles.stat}>
                    <MoneyIcon width={24} height={24} />
                    <Text size="h4">{player.money}</Text>
                </div>
                <div className={styles.stat}>
                    <WeightIcon width={24} height={24} />
                    <Text size="h4">{calcInventoryWeight(inventory)} / {player.capacity}</Text>
                </div>
            </div>
            <div className={styles.subHeader}>
                <Text size="h4">Hand</Text>
            </div>
            <div className={styles.hand} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                <ArrayDisplay elements={inventory.filter(item => item.inventoryItemId === player.activeInventoryItemId).map(item => (
                    <HandItem item={item} key={item.inventoryItemId} />
                ))} ifEmpty={<Text size="h4">Empty hand</Text>} />
            </div>
        </div>
    )
}

export default PlayerUI