import React from "react"
import useBlur from "../../hooks/useBlur"
import Asset from "../../components/SVG/Asset"
import { PlayerContext } from "../../providers/game/PlayerProvider"
import { FloorContext } from "../../providers/FloorProvider"
import type { EnemyType } from "../../types/api/models/building"
import type { AssetType } from "../../types/asset"
import { useMutation } from "@tanstack/react-query"
import { useItemMutation } from "../../api/player"
import styles from "./fight.module.css"

const mapEnemyTypeToAssetType = (enemyType: EnemyType): AssetType => {
    switch (enemyType) {
        case 'Zombie':
            return 'zombie'
        case 'Skeleton':
            return 'skeleton'
        case 'Dragon':
            return 'dragon'
    }
}

const FightScreen = () => {
    useBlur(true)

    const player = React.useContext(PlayerContext)!.player!
    const floor = React.useContext(FloorContext)!.floor!

    const { mutateAsync: useItemAsync } = useMutation(useItemMutation(player.playerId, floor.floorId))

    const enemy = floor.floorItems.filter(item => item.floorItemType === "Enemy").filter(enemy => enemy.positionX === player.subPositionX && enemy.positionY === player.subPositionY)[0]

    const handleClick = () => {
        // eslint-disable-next-line react-hooks/rules-of-hooks
        useItemAsync()
    }
    
    return (
        <div className={styles.container}>
            <div className={styles.entityContainer}>
                <svg width={512} height={512} viewBox="0 0 512 512">
                    <Asset assetType="player" x={0} y={0} width={512} height={512} />
                </svg>
                <span className={styles.entityText}>{player.health} / {player.maxHealth}</span>
            </div>
            <div className={styles.entityContainer} onClick={handleClick}>
                <svg width={512} height={512} viewBox="0 0 512 512">
                    <Asset assetType={mapEnemyTypeToAssetType(enemy.enemy!.enemyType)} x={0} y={0} width={512} height={512} />
                </svg>
                <span className={styles.entityText}>{enemy.enemy?.health} / ?</span>
            </div>
        </div>
    )
}

export default FightScreen
