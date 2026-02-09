import React from 'react'
import type { Blueprint } from '../../types/api/models/blueprint'
import { itemIdToAssetType } from '../../utils/item'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import { buyBlueprintMutation } from '../../api/blueprint'
import useNotification from '../../hooks/useNotification'
import useLock from '../../hooks/useLock'
import Item from './Item'

type BlueprintItemProps = {
    blueprint: Blueprint
}

const BlueprintItem: React.FC<BlueprintItemProps> = ({ blueprint }) => {
    const {genericError} = useNotification()
    const handleLock = useLock()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const { mutateAsync: buyBlueprintAsync } = useMutation(buyBlueprintMutation(playerId, blueprint.blueprintId, genericError))

    const handleClick = async () => {
        await handleLock(async () => {
            await buyBlueprintAsync()
        })
    }

    return (
        <Item tooltipHeading={blueprint.item.name}
            tooltipText={blueprint.item.description}
            assetType={itemIdToAssetType(blueprint.item.itemId)}
            price={blueprint.price}
            onClick={handleClick} />
    )
}

export default BlueprintItem