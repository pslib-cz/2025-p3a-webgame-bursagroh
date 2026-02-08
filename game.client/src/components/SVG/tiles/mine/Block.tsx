import React, { type JSX } from 'react'
import type { AssetProps } from '../../../../types'
import TileSelector from '../../TileSelector'
import Asset from '../../Asset'
import useMine from '../../../../hooks/useMine'

type BlockProps = {
    blockType: "wooden_frame" | "rock" | "copper_ore" | "iron_ore" | "gold_ore" | "silver_ore" | "unobtainium_ore"
    health: number
    maxHealth: number
} & AssetProps

const Block: React.FC<BlockProps> = ({ x, y, width, height, blockType, health, maxHealth }) => {
    const mine = useMine()

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
            <TileSelector width={width} height={height} x={x} y={y} tileType={blockType} onClick={() => mine(x, y)} />
            {breakPattern}
        </>
    )
}

export default Block