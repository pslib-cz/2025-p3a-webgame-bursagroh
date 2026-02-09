import React from 'react'
import type { Blueprint } from '../../types/api/models/blueprint'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import { craftBlueprintMutation } from '../../api/blueprint'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'
import WeightIcon from '../../icons/WeightIcon'
import styles from './craftingItem.module.css'
import Tooltip from '../Tooltip'
import useNotification from '../../hooks/useNotification'
import useLock from '../../hooks/useLock'

type CraftingItemProps = {
    blueprint: Blueprint
}

const CraftingItem: React.FC<CraftingItemProps> = ({ blueprint }) => {
    const {genericError} = useNotification()
    const handleLock = useLock()

    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const { mutateAsync: craftBlueprintAsync } = useMutation(craftBlueprintMutation(playerId, blueprint.blueprintId, genericError))

    const handleClick = async () => {
        await handleLock(async () => {
            await craftBlueprintAsync()
        })
    }

    return (
        <Tooltip heading={blueprint.item.name} text={blueprint.item.description}>
            <div className={styles.container} onClick={handleClick}>
                <svg width="128" height="128" viewBox="0 0 128 128">
                    <Asset assetType={itemIdToAssetType(blueprint.item.itemId)} width={128} height={128} />
                </svg>
                <div className={styles.weight}>
                    <WeightIcon className={styles.weightIcon} width={24} height={24} />
                    <span className={styles.weightText}>{blueprint.item.weight}</span>
                </div>
            </div>
        </Tooltip>
    )
}

export default CraftingItem