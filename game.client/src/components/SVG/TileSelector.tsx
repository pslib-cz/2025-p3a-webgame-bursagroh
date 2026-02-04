import React from 'react'
import type { AssetProps, TileType } from '../../types'
import Asset from './Asset'

type TileSelectorProps = {
    tileType: TileType
} & AssetProps & Omit<React.SVGProps<SVGUseElement>, "x" | "y" | "width" | "height">

const TileSelector: React.FC<TileSelectorProps> = ({width, height, x, y, tileType, ...props}) => {
    switch (tileType) {
        case 'rock':
            return (
                <Asset assetType='rock' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wooden_frame':
            return (
                <Asset assetType='wooden_frame' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'copper_ore':
            return (
                <Asset assetType='copper_ore' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'iron_ore':
            return (
                <Asset assetType='iron_ore' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'gold_ore':
            return (
                <Asset assetType='gold_ore' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'silver_ore':
            return (
                <Asset assetType='silver_ore' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'unobtainium_ore':
            return (
                <Asset assetType='unobtainium_ore' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'bank':
            return (
                <Asset assetType='bank' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'blacksmith':
            return (
                <Asset assetType='blacksmith' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'fountain':
            return (
                <Asset assetType='fountain' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'mine':
            return (
                <Asset assetType='mine' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'restaurant':
            return (
                <Asset assetType='restaurant' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'road':
            return (
                <Asset assetType='road' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'road-vertical':
            return (
                <Asset assetType='road-vertical' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'road-horizontal':
            return (
                <Asset assetType='road-horizontal' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-straight-top':
            return (
                <Asset assetType='abandoned-straight-top' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-straight-right':
            return (
                <Asset assetType='abandoned-straight-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-straight-bottom':
            return (
                <Asset assetType='abandoned-straight-bottom' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-straight-left':
            return (
                <Asset assetType='abandoned-straight-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-straight-top':
            return (
                <Asset assetType='abandoned-trap-straight-top' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-straight-right':
            return (
                <Asset assetType='abandoned-trap-straight-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-straight-bottom':
            return (
                <Asset assetType='abandoned-trap-straight-bottom' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-straight-left':
            return (
                <Asset assetType='abandoned-trap-straight-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-corner-top-left':
            return (
                <Asset assetType='abandoned-corner-top-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-corner-top-right':
            return (
                <Asset assetType='abandoned-corner-top-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-corner-bottom-left':
            return (
                <Asset assetType='abandoned-corner-bottom-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-corner-bottom-right':
            return (
                <Asset assetType='abandoned-corner-bottom-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-corner-top-left':
            return (
                <Asset assetType='abandoned-trap-corner-top-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-corner-top-right':
            return (
                <Asset assetType='abandoned-trap-corner-top-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-corner-bottom-left':
            return (
                <Asset assetType='abandoned-trap-corner-bottom-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'abandoned-trap-corner-bottom-right':
            return (
                <Asset assetType='abandoned-trap-corner-bottom-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'grass':
            return (
                <Asset assetType='grass' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'floor':
            return (
                <Asset assetType='floor' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-top':
            return (
                <Asset assetType='wall-top' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-right':
            return (
                <Asset assetType='wall-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-bottom':
            return (
                <Asset assetType='wall-bottom' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-left':
            return (
                <Asset assetType='wall-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-top-left':
            return (
                <Asset assetType='wall-top-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-top-right':
            return (
                <Asset assetType='wall-top-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-bottom-left':
            return (
                <Asset assetType='wall-bottom-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-bottom-right':
            return (
                <Asset assetType='wall-bottom-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-left-top':
            return (
                <Asset assetType='wall-door-left-top' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-left-right':
            return (
                <Asset assetType='wall-door-left-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-left-bottom':
            return (
                <Asset assetType='wall-door-left-bottom' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-left-left':
            return (
                <Asset assetType='wall-door-left-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-right-top':
            return (
                <Asset assetType='wall-door-right-top' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-right-right':
            return (
                <Asset assetType='wall-door-right-right' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-right-bottom':
            return (
                <Asset assetType='wall-door-right-bottom' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wall-door-right-left':
            return (
                <Asset assetType='wall-door-right-left' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'stair':
            return (
                <Asset assetType='stairs' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'empty':
            return (
                <Asset assetType='empty' x={x} y={y} width={width} height={height} {...props}/>
            )
        case 'zombie':
            return (
                <Asset assetType='zombie' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'skeleton':
            return (
                <Asset assetType='skeleton' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'dragon':
            return (
                <Asset assetType='dragon' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wooden_sword':
            return (
                <Asset assetType='wooden_sword' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wooden_pickaxe':
            return (
                <Asset assetType='wooden_pickaxe' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'wood':
            return (
                <Asset assetType='wood' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'rock_item':
            return (
                <Asset assetType='rock_item' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'copper':
            return (
                <Asset assetType='copper' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'iron':
            return (
                <Asset assetType='iron' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'silver':
            return (
                <Asset assetType='silver' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'gold':
            return (
                <Asset assetType='gold' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'unobtainium':
            return (
                <Asset assetType='unobtainium' x={x} y={y} width={width} height={height} {...props} />
            )
        case 'chest':
            return (
                <Asset assetType='chest' x={x} y={y} width={width} height={height} {...props} />
            )
    }
}

export default TileSelector