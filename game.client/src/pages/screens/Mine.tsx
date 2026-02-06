import React from "react"
import useBlur from "../../hooks/useBlur"
import useMap from "../../hooks/useMap"
import styles from "./mine.module.css"
import { PlayerContext } from "../../providers/game/PlayerProvider"
import GroundItem from "../../components/item/GroundItem"
import { groupFloorItems } from "../../utils/floor"
import ConditionalDisplay from "../../components/wrappers/ConditionalDisplay"
import { getMineItemsQuery } from "../../api/mine"
import { useQuery } from "@tanstack/react-query"

const MineScreen = () => {
    useBlur(false)
    useMap("mine")

    const player = React.useContext(PlayerContext)!.player!

    const { data: mineItems, isError, isPending, isSuccess } = useQuery(getMineItemsQuery(player.playerId, player.mineId))

    if (isError) {
        return <div>Error loading mine items.</div>
    }

    if (isPending) {
        return <div>Loading mine items...</div>
    }

    if (isSuccess) {
        const items = mineItems.filter(item => item.positionX === player.subPositionX && item.positionY === player.subPositionY).map(item => ({ floorItemId: item.floorItemId, item: item.itemInstance }))
        const groupedItems = groupFloorItems(items)

        return (
            <ConditionalDisplay condition={items.length > 0}>
                <div className={styles.container}>
                    <div className={styles.groundContainer}>
                        <span className={styles.heading}>Ground</span>
                        <div className={styles.itemContainer}>
                            {Object.entries(groupedItems).map(([itemString, itemIds]) => (
                                <GroundItem items={items.filter(item => itemIds.includes(item.floorItemId))!} key={itemString} />
                            ))}
                        </div>
                    </div>
                </div>
            </ConditionalDisplay>
        )
    }
}

export default MineScreen
