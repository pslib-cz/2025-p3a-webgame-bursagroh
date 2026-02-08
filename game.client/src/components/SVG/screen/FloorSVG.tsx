import React from 'react'
import SVGDisplay from '../../SVGDisplay'
import Asset from '../Asset'
import { PlayerContext } from '../../../providers/global/PlayerProvider'
import { FloorContext } from '../../../providers/game/FloorProvider'
import type { EnemyType } from '../../../types/api/models/building'
import styles from './floorSVG.module.css'
import Floor from '../Floor'
import Enemy from '../tiles/floor/Enemy'
import FloorTile from '../tiles/floor/FloorTile'
import { itemIdToAssetType } from '../../../utils/item'
import Tooltip from '../../Tooltip'

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
            {floor.floorItems.filter((item) => item.floorItemType === "Stair").map((item) => (
                <FloorTile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} floorTileType='stair' />
            ))}
            {floor.floorItems.filter((item) => item.floorItemType === "Chest").map((item) => (
                <FloorTile z-index={100} key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} floorTileType='chest' />
            ))}
            {floor.floorItems.filter((item) => item.floorItemType === "Item").map((item) => (
                <Asset key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} assetType={itemIdToAssetType(item!.itemInstance!.item!.itemId)} pointerEvents="none" />
            ))}
            {floor.floorItems.filter((item) => item.floorItemType === "Enemy").map((item) => (
                <Enemy key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} enemyType={mapEnemyType(item!.enemy!.enemyType)} />
            ))}
            <Tooltip heading='Player' text={`Player is located at x: ${player.subPositionX} y: ${player.subPositionY}`}>
                <Asset assetType='player' x={player.subPositionX} y={player.subPositionY} width={1} height={1} />
            </Tooltip>
        </SVGDisplay>
    )
}

export default FloorSVG