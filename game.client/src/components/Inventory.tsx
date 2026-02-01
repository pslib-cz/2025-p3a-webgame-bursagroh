import React from 'react'
// import { MineIdContext } from "../providers/MineIdProvider"
// import { BuildingIdContext } from "../providers/BuildingIdProvider"
// import { LayerContext } from "../providers/LayerProvider"
import styles from "./inventory.module.css"
import CloseIcon from '../assets/icons/CloseIcon'
import InventoryItem from './item/InventoryItem'
import { countInventoryItems, removeEquippedItemFromInventory } from '../utils/inventory'
import { PlayerContext } from '../providers/game/PlayerProvider'
import { InventoryContext } from '../providers/game/InventoryProvider'
import { IsOpenInventoryContext } from '../providers/game/IsOpenInventoryProvider'
import ConditionalDisplay from './wrappers/ConditionalDisplay'

const Inventory = () => {
    // const mineId = React.useContext(MineIdContext)!.mineId
    // const buildingId = React.useContext(BuildingIdContext)!.buildingId
    // const layer = React.useContext(LayerContext)!.layer


    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const {isOpen} = React.useContext(IsOpenInventoryContext)!

    // const {mutateAsync: dropItemAsync} = useMutation(dropItemMutation(playerId, mineId ?? -1, buildingId ?? -1, layer ?? -1))

    // const handleDropItem = async (inventoryItemId: number) => {
    //     await dropItemAsync(inventoryItemId)
    // }

    const updatedInventory = removeEquippedItemFromInventory([...inventory], player.activeInventoryItemId)
    const inventoryItems = countInventoryItems(updatedInventory)

    return (
        <ConditionalDisplay condition={isOpen}>
            <div className={styles.container}>
                <div className={styles.header}>
                    <span className={styles.heading}>Inventory</span>
                    <CloseIcon className={styles.close} width={24} height={24} />
                </div>
                <div className={styles.itemContainer}>
                    {Object.entries(inventoryItems).map(([itemId, count]) => (
                        <InventoryItem item={updatedInventory.find(item => item.itemInstance.item.itemId === Number(itemId))!} count={count} key={itemId} />
                    ))}
                </div>
            </div>
        </ConditionalDisplay>
    )
}

export default Inventory