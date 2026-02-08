import React from 'react'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import type { FloorItemInstance } from '../../types/api/models/building'
import { useMutation } from '@tanstack/react-query'
import { pickItemMutation } from '../../api/player'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import ConditionalDisplay from '../wrappers/ConditionalDisplay'
import WeightIcon from '../../assets/icons/WeightIcon'
import styles from './groundItem.module.css'
import Tooltip from '../Tooltip'
import useNotification from '../../hooks/useNotification'

type GroundItemProps = {
    items: {floorItemId: number, item: FloorItemInstance}[]
}

const GroundItem: React.FC<GroundItemProps> = ({ items }) => {
    const {genericError} = useNotification()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const {mutateAsync: pickItemAsync} = useMutation(pickItemMutation(playerId, genericError))

    const handleClick = () => {
        pickItemAsync(items[0].floorItemId)
    }

    return (
        <Tooltip heading={items[0].item.item.name} text={items[0].item.item.description}>
            <div className={styles.container} onClick={handleClick}>
                <svg width="128" height="128" viewBox="0 0 128 128">
                    <Asset assetType={itemIdToAssetType(items[0].item.item.itemId)} width={128} height={128} />
                </svg>
                <ConditionalDisplay condition={items[0].item.durability !== 0}>
                    <span className={styles.durability}>{items[0].item.durability}</span>
                </ConditionalDisplay>
                <div className={styles.weight}>
                    <WeightIcon className={styles.weightIcon} width={24} height={24} />
                    <span className={styles.weightText}>{items[0].item.item.weight}</span>
                </div>
                <span className={styles.amount}>{items.length}x</span>
            </div>
        </Tooltip>
    )
}

export default GroundItem