import React from 'react'
import type { AssetProps } from "../../../../types/asset"
import useMove from '../../../../hooks/useMove'
import useLink from '../../../../hooks/useLink'
import Asset from '../../Asset'

type BuildingProps = {
    buildingType: | "bank"
    | "blacksmith"
    | "fountain"
    | "mine"
    | "restaurant"
    | "abandoned-straight-top"
    | "abandoned-straight-right"
    | "abandoned-straight-bottom"
    | "abandoned-straight-left"
    | "abandoned-trap-straight-top"
    | "abandoned-trap-straight-right"
    | "abandoned-trap-straight-bottom"
    | "abandoned-trap-straight-left"
    | "abandoned-corner-top-left"
    | "abandoned-corner-top-right"
    | "abandoned-corner-bottom-left"
    | "abandoned-corner-bottom-right"
    | "abandoned-trap-corner-top-left"
    | "abandoned-trap-corner-top-right"
    | "abandoned-trap-corner-bottom-left"
    | "abandoned-trap-corner-bottom-right"
} & AssetProps

const Building: React.FC<BuildingProps> = ({x, y, width, height, buildingType}) => {
    const handleMove = useMove()
    const moveToPage = useLink()

    const handleClick = async () => {
        await handleMove(x, y)

        switch (buildingType) {
            case 'bank':
                await moveToPage("bank")
                break
            case 'blacksmith':
                await moveToPage("blacksmith")
                break
            case 'fountain':
                await moveToPage("fountain")
                break
            case 'mine':
                await moveToPage("mine")
                break
            case 'restaurant':
                await moveToPage("restaurant")
                break
            case 'abandoned-straight-top':
            case 'abandoned-straight-right':
            case 'abandoned-straight-bottom':
            case 'abandoned-straight-left':
            case 'abandoned-trap-straight-top':
            case 'abandoned-trap-straight-right':
            case 'abandoned-trap-straight-bottom':
            case 'abandoned-trap-straight-left':
            case 'abandoned-corner-top-left':
            case 'abandoned-corner-top-right':
            case 'abandoned-corner-bottom-left':
            case 'abandoned-corner-bottom-right':
            case 'abandoned-trap-corner-top-left':
            case 'abandoned-trap-corner-top-right':
            case 'abandoned-trap-corner-bottom-left':
            case 'abandoned-trap-corner-bottom-right':
                await moveToPage("floor")
                break
        }
    }

    return (
        <Asset width={width} height={height} x={x} y={y} assetType={buildingType} onClick={handleClick} />
    )
}

export default Building