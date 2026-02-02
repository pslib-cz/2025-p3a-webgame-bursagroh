import React from 'react'
import CraftingItem from './item/CraftingItem'
import type { Blueprint } from '../types/api/models/blueprint'
import Asset from './SVG/Asset'
import { itemIdToAssetType } from '../utils/item'

type CraftingProps = {
    blueprint: Blueprint
}

const Crafting: React.FC<CraftingProps> = ({ blueprint }) => {
    return (
        <div>
            <CraftingItem blueprint={blueprint} />
            {blueprint.craftings.map((crafting) => (
                <div key={crafting.item.itemId}>
                    <svg width="128" height="128" viewBox="0 0 128 128">
                        <Asset assetType={itemIdToAssetType(crafting.item.itemId)} />
                    </svg>
                    <span>{crafting.amount}x</span>
                </div>
            ))}
        </div>
    )
}

export default Crafting