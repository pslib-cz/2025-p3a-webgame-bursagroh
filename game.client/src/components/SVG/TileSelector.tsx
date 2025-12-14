import React from 'react'
import type { AssetProps, TileType } from '../../types'
import Rock from '../../assets/tiles/blocks/Rock'
import Bank from '../../assets/tiles/buildings/Bank'
import Blacksmith from '../../assets/tiles/buildings/Blacksmith'
import Fountain from '../../assets/tiles/buildings/Fountain'
import Mine from '../../assets/tiles/buildings/Mine'
import Restaurant from '../../assets/tiles/buildings/Restaurant'

type TileSelectorProps = {
    tileType: TileType
} & AssetProps & Omit<React.SVGProps<SVGSVGElement>, "x" | "y" | "width" | "height" | "viewBox" | "xmlns">

const TileSelector: React.FC<TileSelectorProps> = ({width, height, x, y, tileType, ...props}) => {
    switch (tileType) {
        case 'rock':
            return (
                <Rock {...props} x={x} y={y} width={width} height={height} />
            )
        case 'bank':
            return (
                <Bank {...props} x={x} y={y} width={width} height={height} />
            )
        case 'blacksmith':
            return (
                <Blacksmith {...props} x={x} y={y} width={width} height={height} />
            )
        case 'fountain':
            return (
                <Fountain {...props} x={x} y={y} width={width} height={height} />
            )
        case 'mine':
            return (
                <Mine {...props} x={x} y={y} width={width} height={height} />
            )
        case 'restaurant':
            return (
                <Restaurant {...props} x={x} y={y} width={width} height={height} />
            )
    }
}

export default TileSelector