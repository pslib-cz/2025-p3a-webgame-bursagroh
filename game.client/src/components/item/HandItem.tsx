import React from 'react'
import type { InventoryItem } from '../../types/api/models/player'
import Asset from '../SVG/Asset'
import { itemIdToAssetType } from '../../utils/item'
import ConditionalDisplay from '../wrappers/ConditionalDisplay'
import styles from './handItem.module.css'
import { useMutation } from '@tanstack/react-query'
import { useItemMutation } from '../../api/player'
import { PlayerContext } from '../../providers/game/PlayerProvider'

type HandItemProps = {
    item: InventoryItem
}

const HandItem: React.FC<HandItemProps> = ({ item }) => {
    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: useItemAsync } = useMutation(useItemMutation(player.playerId))

    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", item.inventoryItemId.toString())
    }

    const handleClick = () => {
        if (item.itemInstance.item.itemType !== "Potion") return

        // eslint-disable-next-line react-hooks/rules-of-hooks
        useItemAsync()
    }

    return (
        <div className={styles.container} draggable onDragStart={handleOnDragStart} onClick={handleClick}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(item.itemInstance.item.itemId)} width={128} height={128} />
            </svg>
            <ConditionalDisplay condition={item.itemInstance.durability !== 0}>
                <span className={styles.durability}>{item.itemInstance.durability}</span>
            </ConditionalDisplay>
        </div>
    )
}

export default HandItem