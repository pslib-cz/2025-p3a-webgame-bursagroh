import React from 'react'
import type { AssetProps } from "../../../../types/asset"
import useMove from '../../../../hooks/useMove'
import Asset from '../../Asset'

type RoadProps = {
    roadType: "road-vertical" | "road-horizontal" | "road"
} & AssetProps

const Road: React.FC<RoadProps> = ({ roadType, x, y, width, height }) => {
    const handleMove = useMove()

    return (
        <Asset width={width} height={height} x={x} y={y} assetType={roadType} onClick={() => handleMove(x, y)} />
    )
}

export default Road