import ConditionalDisplay from '../../components/wrappers/ConditionalDisplay'
import useBlur from '../../hooks/useBlur'
import useMap from '../../hooks/useMap'
import FloorProvider, { FloorContext } from '../../providers/game/FloorProvider'
import { PlayerContext } from '../../providers/global/PlayerProvider'
import React from 'react'
import { groupFloorItems } from '../../utils/floor'
import GroundItem from '../../components/item/GroundItem'
import styles from './floor.module.css'
import useKeyboard from '../../hooks/useKeyboard'
import useKeyboardMove from '../../hooks/useKeyboardMove'
import ProviderGroupLoadingWrapper from '../../components/wrappers/ProviderGroupLoadingWrapper'
import type { TLoadingWrapperContextState } from '../../components/wrappers/LoadingWrapper'
import useLink from '../../hooks/useLink'

const FloorScreenWithContext = () => {
    useBlur(false)
    useMap("floor")
    useKeyboardMove(true)

    const moveToPage = useLink()
  
    const player = React.useContext(PlayerContext)!.player!
    const floor = React.useContext(FloorContext)!.floor!

    const items = floor.floorItems.filter(item => item.floorItemType === "Item").filter(item => item.positionX === player.subPositionX && item.positionY === player.subPositionY).map(item => ({ floorItemId: item.floorItemId, item: item.itemInstance! }))
    const groupedItems = groupFloorItems(items)

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })

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

const FloorScreen = () => {
    return (
        <ProviderGroupLoadingWrapper providers={[FloorProvider]} contextsToLoad={[FloorContext] as Array<React.Context<TLoadingWrapperContextState>>}>
            <FloorScreenWithContext />
        </ProviderGroupLoadingWrapper>
    )
}

export default FloorScreen