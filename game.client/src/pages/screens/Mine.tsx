import React from "react"
import useBlur from "../../hooks/useBlur"
import useMap from "../../hooks/useMap"
import styles from "./mine.module.css"
import { PlayerContext } from "../../providers/global/PlayerProvider"
import GroundItem from "../../components/item/GroundItem"
import { groupFloorItems } from "../../utils/floor"
import ConditionalDisplay from "../../components/wrappers/ConditionalDisplay"
import type { Player } from "../../types/api/models/player"
import RentItem from "../../components/item/RentItem"
import useKeyboard from "../../hooks/useKeyboard"
import { useNavigate } from "react-router"
import useKeyboardMove from "../../hooks/useKeyboardMove"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import MineItemsProvider, { MineItemsContext } from "../../providers/game/MineItemsProvider"
import type { TLoadingWrapperContextState } from "../../components/wrappers/LoadingWrapper"

const isPlayerNextToTable = (player: Player) => {
    const nextToTablePositions = [
        { x: 1, y: -2 },
        { x: 2, y: -2 }
    ]

    return nextToTablePositions.some(pos => pos.x === player.subPositionX && pos.y === player.subPositionY)
}

const MineScreenWithContext = () => {
    useBlur(false)
    useMap("mine")
    useKeyboardMove(true)

    const navigate = useNavigate()

    const player = React.useContext(PlayerContext)!.player!
    const mineItems = React.useContext(MineItemsContext)!.mineItems!

    useKeyboard("Escape", () => {
        navigate("/")
    })

    const items = mineItems.filter(item => item.positionX === player.subPositionX && item.positionY === player.subPositionY).map(item => ({ floorItemId: item.floorItemId, item: item.itemInstance }))
    const groupedItems = groupFloorItems(items)

    return (
        <>
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
            <ConditionalDisplay condition={isPlayerNextToTable(player)}>
                <div className={styles.container}>
                    <div className={styles.groundContainer}>
                        <span className={styles.heading}>Rent a PICK!</span>
                        <div className={styles.itemContainer}>
                            <RentItem />
                        </div>
                    </div>
                </div>
            </ConditionalDisplay>
        </>
    )
}

const MineScreen = () => {
    return (
        <ProviderGroupLoadingWrapper providers={[MineItemsProvider]} contextsToLoad={[MineItemsContext] as Array<React.Context<TLoadingWrapperContextState>>}>
            <MineScreenWithContext />
        </ProviderGroupLoadingWrapper>
    )
}

export default MineScreen
