import React from 'react'
import type { AssetProps } from "../../../../types/asset"
import useMove from '../../../../hooks/useMove'
import Asset from '../../Asset'

type FloorTileProps = {
    floorTileType: "stairs" | "chest" | "floor" | "wall-top" | "wall-bottom" | "wall-left" | "wall-right" | "wall-top-left" | "wall-top-right" | "wall-bottom-left" | "wall-bottom-right" 
    | "wall-door-left-top" | "wall-door-left-right" | "wall-door-left-bottom" | "wall-door-left-left" | "wall-door-right-top" | "wall-door-right-right" | "wall-door-right-bottom" | "wall-door-right-left"
} & AssetProps

const FloorTile: React.FC<FloorTileProps> = ({ x, y, width, height, floorTileType }) => {
    const handleMove = useMove()

    return (
        <Asset width={width} height={height} x={x} y={y} assetType={floorTileType} onClick={() => handleMove(x, y)} />
    )
}

export default FloorTile