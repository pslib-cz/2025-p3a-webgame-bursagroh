import React from 'react'
import styles from "./inventory.module.css"
import CloseIcon from '../icons/CloseIcon'
import InventoryItem from './item/InventoryItem'
import { groupInventoryItems, removeEquippedItemFromInventory } from '../utils/inventory'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { InventoryContext } from '../providers/game/InventoryProvider'
import { IsOpenInventoryContext } from '../providers/game/IsOpenInventoryProvider'
import ConditionalDisplay from './wrappers/ConditionalDisplay'
import ArrayDisplay from './wrappers/ArrayDisplay'
import { useMutation } from '@tanstack/react-query'
import { equipItemMutation } from '../api/player'
import useNotification from '../hooks/useNotification'
import Text from './Text'

const Inventory = () => {
    const {genericError} = useNotification()
    
    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const {isOpen, setIsOpen} = React.useContext(IsOpenInventoryContext)!

    const {mutateAsync: equipItemAsync} = useMutation(equipItemMutation(player.playerId, genericError))

    const updatedInventory = removeEquippedItemFromInventory([...inventory], player.activeInventoryItemId)
    const inventoryItems = groupInventoryItems(updatedInventory)

    const handleCloseInventory = () => {
        setIsOpen(false)
    }

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault()
        const data = event.dataTransfer.getData("text/plain")
        if (data.startsWith("hand_")) {
            equipItemAsync(null)
        }
    }

    return (
        <ConditionalDisplay condition={isOpen}>
            <div className={styles.container}>
                <div className={styles.header}>
                    <Text size="h3">Inventory</Text>
                    <CloseIcon className={styles.close} width={24} height={24} onClick={handleCloseInventory} />
                </div>
                <div className={styles.itemContainer} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()} style={{gridTemplateColumns: `repeat(${Math.max(Math.min(Object.keys(inventoryItems).length, 5), 1)}, max-content)`}}>
                    <ArrayDisplay elements={Object.entries(inventoryItems).map(([itemString, items]) => (
                        <InventoryItem items={updatedInventory.filter(item => items.includes(item.inventoryItemId))!} key={itemString} />
                    ))} ifEmpty={<Text size="h4">Empty inventory</Text>} />
                </div>
            </div>
        </ConditionalDisplay>
    )
}

export default Inventory