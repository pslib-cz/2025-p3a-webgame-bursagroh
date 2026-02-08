import React from "react"
import useBlur from "../../hooks/useBlur"
import Asset from "../../components/SVG/Asset"
import { PlayerContext } from "../../providers/global/PlayerProvider"
import FloorProvider, { FloorContext } from "../../providers/game/FloorProvider"
import styles from "./fight.module.css"
import useUse from "../../hooks/useUse"
import useKeyboard from "../../hooks/useKeyboard"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import type { TLoadingWrapperContextState } from '../../types/context'
import useLink from "../../hooks/useLink"
import { mapEnemyType } from "../../utils/enemy"

const FightScreenWithContext = () => {
    useBlur(true)

    const handleUse = useUse()
    const moveToPage = useLink()

    const player = React.useContext(PlayerContext)!.player!
    const floor = React.useContext(FloorContext)!.floor!

    const enemies = floor.floorItems.filter(item => item.floorItemType === "Enemy").filter(enemy => enemy.positionX === player.subPositionX && enemy.positionY === player.subPositionY)
    const enemy = enemies.length > 0 ? enemies[0] : null

    const handleClick = async () => {
        await handleUse()
    }

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })

    if (!enemy) {
        return (
            <div className={styles.container}>
                <div className={styles.entityContainer}>
                    <svg width={512} height={512} viewBox="0 0 512 512">
                        <Asset assetType="player" x={0} y={0} width={512} height={512} />
                    </svg>
                    <span className={styles.entityText}>{player.health} / {player.maxHealth}</span>
                </div>
                <div className={styles.entityContainer} />
            </div>
        )
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
                    <Asset assetType={mapEnemyType(enemy.enemy!.enemyType)} x={0} y={0} width={512} height={512} />
                </svg>
                <span className={styles.entityText}>{enemy.enemy!.health} / {enemy.enemy!.maxHealth}</span>
            </div>
        </div>
    )
}

const FightScreen = () => {
    return (
        <ProviderGroupLoadingWrapper providers={[FloorProvider]} contextsToLoad={[FloorContext] as Array<React.Context<TLoadingWrapperContextState>>}>
            <FightScreenWithContext />
        </ProviderGroupLoadingWrapper>
    )
}

export default FightScreen
