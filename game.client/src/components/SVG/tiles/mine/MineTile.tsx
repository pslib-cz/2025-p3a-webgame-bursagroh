import React from 'react'
import type { AssetProps } from '../../../../types'
import TileSelector from '../../TileSelector'
import useMove from '../../../../hooks/useMove'

type MineTileProps = {
    mineTileType: "empty"
} & AssetProps

const MineTile: React.FC<MineTileProps> = ({x, y, width, height, mineTileType}) => {
    const handleMove = useMove()

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={mineTileType} onClick={() => handleMove(x, y, true)} />
    )
}

export default MineTile