import OpenIcon from '../assets/icons/OpenIcon'
import HeartIcon from '../assets/icons/HeartIcon'
import MoneyIcon from '../assets/icons/MoneyIcon'
import WeightIcon from '../assets/icons/WeightIcon'
import styles from './playerUI.module.css'
import React from 'react'
import { calcInventoryWeight } from '../utils/inventory'
import { PlayerContext } from '../providers/game/PlayerProvider'
import { InventoryContext } from '../providers/game/InventoryProvider'
import { equipItemMutation } from '../api/player'
import { useMutation } from '@tanstack/react-query'
import HandItem from './item/HandItem'
import { IsOpenInventoryContext } from '../providers/game/IsOpenInventoryProvider'

const PlayerUI = () => {
    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const { setIsOpen } = React.useContext(IsOpenInventoryContext)!

    const {mutateAsync: equipItemAsync} = useMutation(equipItemMutation(player.playerId))

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault()
        const inventoryItemId = Number(event.dataTransfer.getData("text/plain"))
        equipItemAsync(inventoryItemId)
    }

    const handleOpenInventory = () => {
        setIsOpen(prev => !prev)
    }

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <span className={styles.heading}>Player</span>
                <OpenIcon width={24} height={24} onClick={handleOpenInventory} />
            </div>
            <div className={styles.statContainer}>
                <div className={styles.stat}>
                    <HeartIcon width={24} height={24} />
                    <span>{player.health} / ?</span>
                </div>
                <div className={styles.stat}>
                    <MoneyIcon width={24} height={24} />
                    <span>{player.money}</span>
                </div>
                <div className={styles.stat}>
                    <WeightIcon width={24} height={24} />
                    <span>{calcInventoryWeight(inventory)} / {player.capacity}</span>
                </div>
            </div>
            <div className={styles.subHeader}>
                <span className={styles.subHeading}>Hand</span>
            </div>
            <div className={styles.hand} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                {inventory.filter(item => item.inventoryItemId === player.activeInventoryItemId).map(item => (
                    <HandItem item={item} key={item.inventoryItemId} />
                ))}
            </div>
        </div>
    )
}

export default PlayerUI