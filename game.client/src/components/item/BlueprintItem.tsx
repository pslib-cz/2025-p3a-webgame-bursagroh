import React from 'react'
import type { Blueprint } from '../../types/api/models/blueprint'
import Asset from '../SVG/Asset'
import { itemIdToAssetType } from '../../utils/item'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import { buyBlueprintMutation } from '../../api/blueprint'
import styles from './blueprintItem.module.css'

type BlueprintItemProps = {
    blueprint: Blueprint
}

const BlueprintItem: React.FC<BlueprintItemProps> = ({ blueprint }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const { mutateAsync: buyBlueprintAsync } = useMutation(buyBlueprintMutation(playerId, blueprint.blueprintId))

    const handleClick = () => {
        buyBlueprintAsync()
    }

    return (
        <div className={styles.container} onClick={handleClick}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(blueprint.item.itemId)} width={128} height={128} />
            </svg>
            <span className={styles.price}>{blueprint.price}$</span>
        </div>
    )
}

export default BlueprintItem