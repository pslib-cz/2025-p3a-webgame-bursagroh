import React from 'react'
import styles from "./inventory.module.css"
import CloseIcon from '../icons/CloseIcon'
import InventoryItem from './item/InventoryItem'
import { groupInventoryItems, removeEquippedItemFromInventory } from '../utils/inventory'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { InventoryContext } from '../providers/game/InventoryProvider'
import { IsOpenInventoryContext } from '../providers/game/IsOpenInventoryProvider'
import ConditionalDisplay from './wrappers/ConditionalDisplay'
import { useMutation } from '@tanstack/react-query'
import { equipItemMutation } from '../api/player'
import useNotification from '../hooks/useNotification'
import Text from './Text'
import ItemContainer from './item/ItemContainer'

const Inventory = () => {
    const { genericError } = useNotification()

    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const { isOpen, setIsOpen } = React.useContext(IsOpenInventoryContext)!

    const { mutateAsync: equipItemAsync } = useMutation(equipItemMutation(player.playerId, genericError))

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
                    <CloseIcon className={styles.close} onClick={handleCloseInventory} />
                </div>
                <div className={styles.itemContainer} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                    <ConditionalDisplay condition={Object.keys(inventoryItems).length > 0} notMet={<Text size="h4">Empty inventory</Text>}>
                        <ItemContainer itemCount={Object.keys(inventoryItems).length}>
                            {Object.entries(inventoryItems).map(([itemString, items]) => (
                                <InventoryItem items={updatedInventory.filter(item => items.includes(item.inventoryItemId))!} key={itemString} />
                            ))}
                        </ItemContainer>
                    </ConditionalDisplay>
                </div>
            </div>
        </ConditionalDisplay>
    )
}

export default Inventory