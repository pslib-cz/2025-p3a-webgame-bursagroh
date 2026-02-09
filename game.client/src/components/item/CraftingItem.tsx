import React from 'react'
import type { Blueprint } from '../../types/api/models/blueprint'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import { craftBlueprintMutation } from '../../api/blueprint'
import { itemIdToAssetType } from '../../utils/item'
import useNotification from '../../hooks/useNotification'
import useLock from '../../hooks/useLock'
import Item from './Item'

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
        <Item tooltipHeading={blueprint.item.name}
            tooltipText={blueprint.item.description}
            assetType={itemIdToAssetType(blueprint.item.itemId)}
            weight={blueprint.item.weight}
            isWeightDown
            onClick={handleClick} />
    )
}

export default CraftingItem