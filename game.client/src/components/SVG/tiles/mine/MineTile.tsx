import React from 'react'
import type { AssetProps } from "../../../../types/asset"
import useMove from '../../../../hooks/useMove'
import Asset from '../../Asset'

type MineTileProps = {
    mineTileType: "empty"
} & AssetProps

const MineTile: React.FC<MineTileProps> = ({x, y, width, height, mineTileType}) => {
    const handleMove = useMove()

    return (
        <Asset width={width} height={height} x={x} y={y} assetType={mineTileType} onClick={() => handleMove(x, y)} />
    )
}

export default MineTile