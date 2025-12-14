import React from "react"
import type { AssetProps, TileType } from "../../types"
import TileSelector from "./TileSelector"

type TileProps = {
    tileType: TileType
    isSelected?: boolean
} & AssetProps

const Tile: React.FC<TileProps> = ({ width, height, x, y, tileType, isSelected = false}) => {
    const handleClick = () => {
        
    }

    return (
        <>
            <rect x={x} y={y} width={width} height={height} stroke={isSelected ? "red" : "none"} strokeWidth={0.05} />
            <TileSelector width={width} height={height} x={x} y={y} tileType={tileType} onClick={handleClick} />
        </>
    )
}

export default Tile
