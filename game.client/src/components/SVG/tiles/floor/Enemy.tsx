import React from 'react'
import TileSelector from '../../TileSelector'
import type { AssetProps, EnemyType } from '../../../../types'
import { useNavigate } from 'react-router'
import useMove from '../../../../hooks/useMove'

type EnemyProps = {
    enemyType: EnemyType
} & AssetProps

const Enemy: React.FC<EnemyProps> = ({ width, height, x, y, enemyType }) => {
    const navigate = useNavigate()
    const handleMove = useMove()

    const handleClick = async () => {
        await handleMove(x, y)

        navigate("/game/fight")
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={enemyType} onClick={handleClick} />
    )
}

export default Enemy