import OpenIcon from '../assets/icons/OpenIcon'
import HeartIcon from '../assets/icons/HeartIcon'
import MoneyIcon from '../assets/icons/MoneyIcon'
import WeightIcon from '../assets/icons/WeightIcon'
import styles from './playerUI.module.css'
import { ActiveItemContext } from '../providers/ActiveItemProvider'
import { PlayerIdContext } from '../providers/PlayerIdProvider'
import { getPlayerInventoryQuery, getPlayerQuery } from '../api/player'
import React from 'react'
import { useQuery } from '@tanstack/react-query'
import Item from './Item'

const PlayerUI = () => {
    const activeItem = React.useContext(ActiveItemContext)!
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = useQuery(getPlayerQuery(playerId))
    const inventory = useQuery(getPlayerInventoryQuery(playerId))

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault()
        const inventoryItemId = Number(event.dataTransfer.getData("text/plain"))
        activeItem.setActiveItemInventoryItemId(prev => prev === inventoryItemId ? null : inventoryItemId)
    }

    if (player.isError || inventory.isError) {
        return <div>Error</div>
    }

    if (player.isPending || inventory.isPending) {
        return <div>Loading...</div>
    }

    if (player.isSuccess && inventory.isSuccess) {
        return (
            <div className={styles.container}>
                <div className={styles.header}>
                    <h3 className={styles.heading}>Player</h3>
                    <OpenIcon width={24} height={24} />
                </div>
                <div className={styles.statContainer}>
                    <div className={styles.stat}>
                        <HeartIcon width={24} height={24} />
                        <span>? / ?</span>
                    </div>
                    <div className={styles.stat}>
                        <MoneyIcon width={24} height={24} />
                        <span>{player.data.money}</span>
                    </div>
                    <div className={styles.stat}>
                        <WeightIcon width={24} height={24} />
                        <span>? / ?</span>
                    </div>
                </div>
                <div className={styles.subHeader}>
                    <h4 className={styles.subHeading}>Hand</h4>
                </div>
                <div className={styles.hand} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                    {inventory.data.filter(item => item.inventoryItemId === activeItem.activeItemInventoryItemId).map(item => (
                        <Item item={item} key={item.inventoryItemId} />
                    ))}
                </div>
            </div>
        )
    }
}

export default PlayerUI