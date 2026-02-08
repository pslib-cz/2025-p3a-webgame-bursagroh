import React from 'react'
// import { MineIdContext } from "../providers/MineIdProvider"
// import { BuildingIdContext } from "../providers/BuildingIdProvider"
// import { LayerContext } from "../providers/LayerProvider"
import styles from "./inventory.module.css"
import CloseIcon from '../assets/icons/CloseIcon'
import InventoryItem from './item/InventoryItem'
import { groupInventoryItems, removeEquippedItemFromInventory } from '../utils/inventory'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { InventoryContext } from '../providers/game/InventoryProvider'
import { IsOpenInventoryContext } from '../providers/game/IsOpenInventoryProvider'
import ConditionalDisplay from './wrappers/ConditionalDisplay'
import ArrayDisplay from './wrappers/ArrayDisplay'

const Inventory = () => {
    // const mineId = React.useContext(MineIdContext)!.mineId
    // const buildingId = React.useContext(BuildingIdContext)!.buildingId
    // const layer = React.useContext(LayerContext)!.layer


    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const {isOpen, setIsOpen} = React.useContext(IsOpenInventoryContext)!

    // const {mutateAsync: dropItemAsync} = useMutation(dropItemMutation(playerId, mineId ?? -1, buildingId ?? -1, layer ?? -1))

    // const handleDropItem = async (inventoryItemId: number) => {
    //     await dropItemAsync(inventoryItemId)
    // }

    const updatedInventory = removeEquippedItemFromInventory([...inventory], player.activeInventoryItemId)
    const inventoryItems = groupInventoryItems(updatedInventory)

    const handleCloseInventory = () => {
        setIsOpen(false)
    }

    return (
        <ConditionalDisplay condition={isOpen}>
            <div className={styles.container}>
                <div className={styles.header}>
                    <span className={styles.heading}>Inventory</span>
                    <CloseIcon className={styles.close} width={24} height={24} onClick={handleCloseInventory} />
                </div>
                <div className={styles.itemContainer}>
                    <ArrayDisplay elements={Object.entries(inventoryItems).map(([itemString, items]) => (
                        <InventoryItem items={updatedInventory.filter(item => items.includes(item.inventoryItemId))!} key={itemString} />
                    ))} ifEmpty={<span className={styles.text}>Empty inventory</span>} />
                </div>
            </div>
        </ConditionalDisplay>
    )
}

export default Inventory