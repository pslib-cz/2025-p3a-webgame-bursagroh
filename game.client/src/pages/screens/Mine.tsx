import React from "react"
import useBlur from "../../hooks/useBlur"
import useMap from "../../hooks/useMap"
import styles from "./mine.module.css"
import { PlayerContext } from "../../providers/global/PlayerProvider"
import GroundItem from "../../components/item/GroundItem"
import { groupFloorItems } from "../../utils/floor"
import ConditionalDisplay from "../../components/wrappers/ConditionalDisplay"
import RentItem from "../../components/item/RentItem"
import useKeyboard from "../../hooks/useKeyboard"
import useKeyboardMove from "../../hooks/useKeyboardMove"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import MineItemsProvider, { MineItemsContext } from "../../providers/game/MineItemsProvider"
import type { TLoadingWrapperContextState } from '../../types/context'
import useLink from "../../hooks/useLink"
import { isPlayerNextToTable } from "../../utils/mine"

const MineScreenWithContext = () => {
    useBlur(false)
    useMap("mine")
    useKeyboardMove(true)

    const moveToPage = useLink()

    const player = React.useContext(PlayerContext)!.player!
    const mineItems = React.useContext(MineItemsContext)!.mineItems!

    useKeyboard("Escape", async () => {
        await moveToPage("root")
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
