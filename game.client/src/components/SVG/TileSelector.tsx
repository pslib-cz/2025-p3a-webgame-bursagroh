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
import JunctionRoad from '../../assets/tiles/buildings/roads/JunctionRoad'
import StraightRoad from '../../assets/tiles/buildings/roads/StraightRoad'
import BuildingStraight from '../../assets/tiles/buildings/building/BuildingStraight'
import BuildingStraightTrap from '../../assets/tiles/buildings/building/BuildingStraightTrap'
import BuildingCorner from '../../assets/tiles/buildings/building/BuildingCorner'
import BuildingCornerTrap from '../../assets/tiles/buildings/building/BuildingCornerTrap'
import Grass from '../../assets/tiles/buildings/Grass'
import Floor from '../../assets/tiles/floors/Floor'
import FloorWall from '../../assets/tiles/floors/FloorWall'
import FloorCorner from '../../assets/tiles/floors/FloorCorner'
import FloorDoorLeft from '../../assets/tiles/floors/FloorDoorLeft'
import FloorDoorRight from '../../assets/tiles/floors/FloorDoorRight'
import FloorStairs from '../../assets/tiles/floors/FloorStairs'
import EmptyBlock from '../../assets/tiles/blocks/EmptyBlock'
import Zombie from '../../assets/tiles/enemies/Zombie'
import Dragon from '../../assets/tiles/enemies/Dragon'
import Skeleton from '../../assets/tiles/enemies/Skeleton'

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
        case 'road':
            return (
                <JunctionRoad {...props} x={x} y={y} width={width} height={height} />
            )
        case 'road-vertical':
            return (
                <StraightRoad {...props} x={x} y={y} width={width} height={height} placement='vertical' />
            )
        case 'road-horizontal':
            return (
                <StraightRoad {...props} x={x} y={y} width={width} height={height} placement='horizontal' />
            )
        case 'abandoned-straight-top':
            return (
                <BuildingStraight {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'abandoned-straight-right':
            return (
                <BuildingStraight {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'abandoned-straight-bottom':
            return (
                <BuildingStraight {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'abandoned-straight-left':
            return (
                <BuildingStraight {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'abandoned-trap-straight-top':
            return (
                <BuildingStraightTrap {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'abandoned-trap-straight-right':
            return (
                <BuildingStraightTrap {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'abandoned-trap-straight-bottom':
            return (
                <BuildingStraightTrap {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'abandoned-trap-straight-left':
            return (
                <BuildingStraightTrap {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'abandoned-corner-top-left':
            return (
                <BuildingCorner {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'abandoned-corner-top-right':
            return (
                <BuildingCorner {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'abandoned-corner-bottom-left':
            return (
                <BuildingCorner {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'abandoned-corner-bottom-right':
            return (
                <BuildingCorner {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'abandoned-trap-corner-top-left':
            return (
                <BuildingCornerTrap {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'abandoned-trap-corner-top-right':
            return (
                <BuildingCornerTrap {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'abandoned-trap-corner-bottom-left':
            return (
                <BuildingCornerTrap {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'abandoned-trap-corner-bottom-right':
            return (
                <BuildingCornerTrap {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'grass':
            return (
                <Grass {...props} x={x} y={y} width={width} height={height} />
            )
        case 'floor':
            return (
                <Floor {...props} x={x} y={y} width={width} height={height} />
            )
        case 'wall-top':
            return (
                <FloorWall {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'wall-right':
            return (
                <FloorWall {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'wall-bottom':
            return (
                <FloorWall {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'wall-left':
            return (
                <FloorWall {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'wall-top-left':
            return (
                <FloorCorner {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'wall-top-right':
            return (
                <FloorCorner {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'wall-bottom-left':
            return (
                <FloorCorner {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'wall-bottom-right':
            return (
                <FloorCorner {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'wall-door-left-top':
            return (
                <FloorDoorLeft {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'wall-door-left-right':
            return (
                <FloorDoorLeft {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'wall-door-left-bottom':
            return (
                <FloorDoorLeft {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'wall-door-left-left':
            return (
                <FloorDoorLeft {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'wall-door-right-top':
            return (
                <FloorDoorRight {...props} x={x} y={y} width={width} height={height} rotation='180deg' />
            )
        case 'wall-door-right-right':
            return (
                <FloorDoorRight {...props} x={x} y={y} width={width} height={height} rotation='270deg' />
            )
        case 'wall-door-right-bottom':
            return (
                <FloorDoorRight {...props} x={x} y={y} width={width} height={height} rotation='0deg' />
            )
        case 'wall-door-right-left':
            return (
                <FloorDoorRight {...props} x={x} y={y} width={width} height={height} rotation='90deg' />
            )
        case 'stair':
            return (
                <FloorStairs {...props} x={x} y={y} width={width} height={height} />
            )
        case 'empty':
            return (
                <EmptyBlock {...props} x={x} y={y} width={width} height={height} />
            )
        case 'zombie':
            return (
                <Zombie {...props} x={x} y={y} width={width} height={height} />
            )
        case 'skeleton':
            return (
                <Skeleton {...props} x={x} y={y} width={width} height={height} />
            )
        case 'dragon':
            return (
                <Dragon {...props} x={x} y={y} width={width} height={height} />
            )
    }
}

export default TileSelector