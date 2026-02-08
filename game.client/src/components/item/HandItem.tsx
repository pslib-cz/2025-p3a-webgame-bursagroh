import React from 'react'
import type { InventoryItem } from '../../types/api/models/player'
import Asset from '../SVG/Asset'
import { itemIdToAssetType } from '../../utils/item'
import ConditionalDisplay from '../wrappers/ConditionalDisplay'
import styles from './handItem.module.css'
import useUse from '../../hooks/useUse'
import Tooltip from '../Tooltip'
import { useMutation } from '@tanstack/react-query'
import { equipItemMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import useNotification from '../../hooks/useNotification'

type HandItemProps = {
    item: InventoryItem
}

const HandItem: React.FC<HandItemProps> = ({ item }) => {
    const handleUse = useUse()
    const {genericError} = useNotification()

    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const {mutateAsync: equipItemAsync} = useMutation(equipItemMutation(playerId, genericError))

    const handleOnDragStart = (event: React.DragEvent<HTMLDivElement>) => {
        event.dataTransfer.setData("text/plain", `hand_${item.inventoryItemId.toString()}`)
    }

    const handleClick = async () => {
        if (item.itemInstance.item.itemType === "Potion") {
            await handleUse()
            return
        }

        await equipItemAsync(null)
    }

    return (
        <Tooltip heading={item.itemInstance.item.name} text={item.itemInstance.item.description}>
            <div className={styles.container} draggable onDragStart={handleOnDragStart} onClick={handleClick}>
                <svg width="128" height="128" viewBox="0 0 128 128">
                    <Asset assetType={itemIdToAssetType(item.itemInstance.item.itemId)} width={128} height={128} />
                </svg>
                <ConditionalDisplay condition={item.itemInstance.durability !== 0}>
                    <span className={styles.durability}>{item.itemInstance.durability}</span>
                </ConditionalDisplay>
            </div>
        </Tooltip>
    )
}

export default HandItem