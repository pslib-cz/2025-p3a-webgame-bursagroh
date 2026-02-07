import React from 'react'
import type { AssetProps } from '../../../../types'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import { useMutation } from '@tanstack/react-query'
import { validMove } from '../../../../utils/player'
import TileSelector from '../../TileSelector'
import { mineMineBlockMutation } from '../../../../api/mine'
import useNotification from '../../../../hooks/useNotification'

type BlockProps = {
    blockType: "wooden_frame" | "rock" | "copper_ore" | "iron_ore" | "gold_ore" | "silver_ore" | "unobtainium_ore"
} & AssetProps

const Block: React.FC<BlockProps> = ({x, y, width, height, blockType}) => {
    const notify = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: mineMineBlockAsync } = useMutation(mineMineBlockMutation(player.playerId, player.mineId, x, y))
    
    const handleClick = () => {
        if (!validMove(player.subPositionX, player.subPositionY, x, y)) {
            notify("Error", "You cannot mine that far.", 1000)
            return
        }

        mineMineBlockAsync()
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={blockType} onClick={handleClick} />
    )
}

export default Block