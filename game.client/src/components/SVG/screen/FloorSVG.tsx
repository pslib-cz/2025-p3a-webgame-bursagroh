import React from 'react'
import SVGDisplay from '../../SVGDisplay'
import Tile from '../Tile'
import Asset from '../Asset'
import { PlayerContext } from '../../../providers/game/PlayerProvider'
import { FloorContext } from '../../../providers/FloorProvider'
import type { TileType } from '../../../types'
import type { EnemyType } from '../../../types/api/models/building'
import styles from './floorSVG.module.css'
import Floor from '../Floor'

const mapEnemyTypeToTileType = (enemyType: EnemyType): TileType => {
    switch (enemyType) {
        case 'Zombie':
            return 'zombie'
        case 'Skeleton':
            return 'skeleton'
        case 'Dragon':
            return 'dragon'
    }
}

const mapItemIdToTileType = (itemId: number): TileType => {
    switch (itemId) {
        case 1:
            return "wood"
        case 2:
            return "rock_item"
        case 3:
            return "copper"
        case 4:
            return "iron"
        case 5:
            return "silver"
        case 6:
            return "gold"
        case 7:
            return "unobtainium"
        case 10:
            return "wooden_sword"
        case 30:
            return "wooden_pickaxe"
        case 39:
            return "wooden_pickaxe"
        default:
            return "empty"
    }
}

const FloorSVG = () => {
    const floor = React.useContext(FloorContext)!.floor
    const player = React.useContext(PlayerContext)!.player!

    if (!floor) {
        return <div>Loading floor...</div>
    }

    return (
        <SVGDisplay className={styles.floor} centerX={player.subPositionX} centerY={player.subPositionY}>
            <Floor />
            {floor.floorItems.map((item) => {
                if (item.floorItemType === "Stair") {
                    return (
                        <Tile z-index={100} key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} tileType='stair' targetLevel={0} targetFloorId={0} />
                    )
                }

                if (item.floorItemType === "Enemy" && item.enemy) {
                    return (
                        <Tile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} tileType={mapEnemyTypeToTileType(item.enemy.enemyType)} targetBuildingId={0} targetLevel={0} />
                    )
                }

                if (item.floorItemType === "Item" && item.itemInstance) {
                    return (
                        <Tile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} tileType={mapItemIdToTileType(item.itemInstance.item.itemId)} targetFloorItemId={item.floorItemId} targetBuildingId={0} targetLevel={0} />
                    )
                }
            })}
            <Asset assetType='player' x={player.subPositionX} y={player.subPositionY} width={1} height={1} />
        </SVGDisplay>
    )
}

export default FloorSVG