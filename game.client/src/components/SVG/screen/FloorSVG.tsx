import React from 'react'
import SVGDisplay from '../../SVGDisplay'
import Asset from '../Asset'
import { PlayerContext } from '../../../providers/game/PlayerProvider'
import { FloorContext } from '../../../providers/FloorProvider'
import type { EnemyType } from '../../../types/api/models/building'
import styles from './floorSVG.module.css'
import Floor from '../Floor'
import Enemy from '../tiles/floor/Enemy'
import FloorTile from '../tiles/floor/FloorTile'
import { itemIdToAssetType } from '../../../utils/item'

const mapEnemyType = (enemyType: EnemyType) => {
    switch (enemyType) {
        case 'Zombie':
            return 'zombie'
        case 'Skeleton':
            return 'skeleton'
        case 'Dragon':
            return 'dragon'
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
            <Floor positionX={player.positionX} positionY={player.positionY} level={floor.level} />
            {floor.floorItems.map((item) => {
                if (item.floorItemType === "Stair") {
                    return (
                        <FloorTile z-index={100} key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} floorTileType='stair' />
                    )
                }

                if (item.floorItemType === "Enemy" && item.enemy) {
                    return (
                        <Enemy key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} enemyType={mapEnemyType(item.enemy.enemyType)} />
                    )
                }

                if (item.floorItemType === "Item" && item.itemInstance) {
                    return (
                        <Asset key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} assetType={itemIdToAssetType(item.itemInstance.item.itemId)} pointerEvents="none" />
                    )
                }

                if (item.floorItemType === "Chest" && item.chest) {
                    return (
                        <FloorTile z-index={100} key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} floorTileType='chest' />
                    )
                }
            })}
            <Asset assetType='player' x={player.subPositionX} y={player.subPositionY} width={1} height={1} />
        </SVGDisplay>
    )
}

export default FloorSVG