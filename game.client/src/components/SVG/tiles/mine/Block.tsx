import React, { type JSX } from 'react'
import type { AssetProps } from '../../../../types'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import { useMutation } from '@tanstack/react-query'
import { validMove } from '../../../../utils/player'
import TileSelector from '../../TileSelector'
import { mineMineBlockMutation } from '../../../../api/mine'
import useNotification from '../../../../hooks/useNotification'
import Asset from '../../Asset'

type BlockProps = {
    blockType: "wooden_frame" | "rock" | "copper_ore" | "iron_ore" | "gold_ore" | "silver_ore" | "unobtainium_ore"
    health: number
    maxHealth: number
} & AssetProps

const Block: React.FC<BlockProps> = ({ x, y, width, height, blockType, health, maxHealth }) => {
    const notify = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const lock = React.useRef(false)

    const { mutateAsync: mineMineBlockAsync } = useMutation(mineMineBlockMutation(player.playerId, player.mineId, x, y))

    const handleClick = async () => {
        if (!validMove(player.subPositionX, player.subPositionY, x, y)) {
            notify("Error", "You cannot mine that far.", 1000)
            return
        }

        if (lock.current) return

        lock.current = true
        try {
            await mineMineBlockAsync()
        } finally {
            lock.current = false
        }
    }

    let breakStage = 0
    if (health < maxHealth) {
        const breakStages = 4
        const damageRatio = (maxHealth - health) / maxHealth
        breakStage = Math.min(breakStages, Math.max(1, Math.ceil(damageRatio * breakStages)))
    }

    let breakPattern: JSX.Element | null = null
    switch (breakStage) {
        case 1:
            breakPattern = <Asset assetType="break_pattern_1" x={x} y={y} width={width} height={height} pointerEvents={"none"} />
            break;
        case 2:
            breakPattern = <Asset assetType="break_pattern_2" x={x} y={y} width={width} height={height} pointerEvents={"none"} />
            break;
        case 3:
            breakPattern = <Asset assetType="break_pattern_3" x={x} y={y} width={width} height={height} pointerEvents={"none"} />
            break;
        case 4:
            breakPattern = <Asset assetType="break_pattern_4" x={x} y={y} width={width} height={height} pointerEvents={"none"} />
            break;
    }

    return (
        <>
            <TileSelector width={width} height={height} x={x} y={y} tileType={blockType} onClick={handleClick} />
            {breakPattern}
        </>
    )
}

export default Block