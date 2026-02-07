import React from 'react'
import TileSelector from '../../TileSelector'
import type { AssetProps } from '../../../../types'
import useMove from '../../../../hooks/useMove'

type RoadProps = {
    roadType: "road-vertical" | "road-horizontal" | "road"
} & AssetProps

const Road: React.FC<RoadProps> = ({ roadType, x, y, width, height }) => {
    const handleMove = useMove()

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={roadType} onClick={() => handleMove(x, y, false)} />
    )
}

export default Road