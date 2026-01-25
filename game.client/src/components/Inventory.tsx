import React from 'react'
import { PlayerIdContext } from "../providers/PlayerIdProvider"
import { /*useMutation,*/ useQuery } from "@tanstack/react-query"
import { /*dropItemMutation,*/ getPlayerInventoryQuery, getPlayerQuery } from "../api/player"
// import { MineIdContext } from "../providers/MineIdProvider"
// import { BuildingIdContext } from "../providers/BuildingIdProvider"
// import { LayerContext } from "../providers/LayerProvider"
import styles from "./inventory.module.css"
import CloseIcon from '../assets/icons/CloseIcon'
import Item from './Item'

const Inventory = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    // const mineId = React.useContext(MineIdContext)!.mineId
    // const buildingId = React.useContext(BuildingIdContext)!.buildingId
    // const layer = React.useContext(LayerContext)!.layer
    const player = useQuery(getPlayerQuery(playerId))
    const inventory = useQuery(getPlayerInventoryQuery(playerId))

    // const {mutateAsync: dropItemAsync} = useMutation(dropItemMutation(playerId, mineId ?? -1, buildingId ?? -1, layer ?? -1))

    // const handleDropItem = async (inventoryItemId: number) => {
    //     await dropItemAsync(inventoryItemId)
    // }

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
                    <h3 className={styles.heading}>Inventory</h3>
                    <CloseIcon className={styles.close} width={24} height={24} />
                </div>
                <div className={styles.itemContainer}>
                    {inventory.data.map((item) => (
                        <Item item={item} key={item.inventoryItemId} />
                    ))}
                </div>
            </div>
        )
    }
}

export default Inventory