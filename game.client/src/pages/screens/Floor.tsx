import ConditionalDisplay from '../../components/wrappers/ConditionalDisplay'
import useBlur from '../../hooks/useBlur'
import useMap from '../../hooks/useMap'
import { FloorContext } from '../../providers/FloorProvider'
import { PlayerContext } from '../../providers/game/PlayerProvider'
import React from 'react'
import { groupFloorItems } from '../../utils/floor'
import GroundItem from '../../components/item/GroundItem'
import styles from './floor.module.css'
import useKeyboard from '../../hooks/useKeyboard'
import { useNavigate } from 'react-router'
import useKeyboardMove from '../../hooks/useKeyboardMove'

const FloorScreen = () => {
    useBlur(false)
    useMap("floor")
    useKeyboardMove(true)

    const navigate = useNavigate()
  
    const player = React.useContext(PlayerContext)!.player!
    const floor = React.useContext(FloorContext)!.floor!

    const items = floor.floorItems.filter(item => item.floorItemType === "Item").filter(item => item.positionX === player.subPositionX && item.positionY === player.subPositionY).map(item => ({ floorItemId: item.floorItemId, item: item.itemInstance! }))
    const groupedItems = groupFloorItems(items)

    useKeyboard("Escape", () => {
        navigate("/")
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

export default FloorScreen