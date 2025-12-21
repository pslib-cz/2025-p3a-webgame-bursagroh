import React from 'react'
import type { AssetProps, TileType } from '../../types'
import Rock from '../../assets/tiles/blocks/Rock'
import Bank from '../../assets/tiles/buildings/Bank'
import Blacksmith from '../../assets/tiles/buildings/Blacksmith'
import Fountain from '../../assets/tiles/buildings/Fountain'
import Mine from '../../assets/tiles/buildings/Mine'
import Restaurant from '../../assets/tiles/buildings/Restaurant'
import WoodenFrame from '../../assets/tiles/blocks/WoodenFrame'
import CopperOre from '../../assets/tiles/blocks/CopperOre'
import IronOre from '../../assets/tiles/blocks/IronOre'
import GoldOre from '../../assets/tiles/blocks/GoldOre'
import SilverOre from '../../assets/tiles/blocks/SilverOre'
import UnobtainiumOre from '../../assets/tiles/blocks/UnobtainiumOre'

type TileSelectorProps = {
    tileType: TileType
} & AssetProps & Omit<React.SVGProps<SVGSVGElement>, "x" | "y" | "width" | "height" | "viewBox" | "xmlns">

const TileSelector: React.FC<TileSelectorProps> = ({width, height, x, y, tileType, ...props}) => {
    switch (tileType) {
        case 'rock':
            return (
                <Rock {...props} x={x} y={y} width={width} height={height} />
            )
        case 'wooden_frame':
            return (
                <WoodenFrame {...props} x={x} y={y} width={width} height={height} />
            )
        case 'copper_ore':
            return (
                <CopperOre {...props} x={x} y={y} width={width} height={height} />
            )
        case 'iron_ore':
            return (
                <IronOre {...props} x={x} y={y} width={width} height={height} />
            )
        case 'gold_ore':
            return (
                <GoldOre {...props} x={x} y={y} width={width} height={height} />
            )
        case 'silver_ore':
            return (
                <SilverOre {...props} x={x} y={y} width={width} height={height} />
            )
        case 'unobtanium_ore':
            return (
                <UnobtainiumOre {...props} x={x} y={y} width={width} height={height} />
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