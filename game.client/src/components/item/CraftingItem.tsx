import React from 'react'
import type { Blueprint } from '../../types/api/models/blueprint'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import { craftBlueprintMutation } from '../../api/blueprint'
import { itemIdToAssetType } from '../../utils/item'
import Asset from '../SVG/Asset'

type CraftingItemProps = {
    blueprint: Blueprint
}

const CraftingItem: React.FC<CraftingItemProps> = ({ blueprint }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: craftBlueprintAsync } = useMutation(craftBlueprintMutation(playerId, blueprint.blueprintId))

    const handleClick = () => {
        craftBlueprintAsync()
    }

    return (
        <div onClick={handleClick}>
            <svg width="128" height="128" viewBox="0 0 128 128">
                <Asset assetType={itemIdToAssetType(blueprint.item.itemId)} />
            </svg>
            <span>{blueprint.item.weight}</span>
        </div>
    )
}

export default CraftingItem