import React from 'react'
import TileSelector from '../TileSelector'
import type { AssetProps, EnemyType } from '../../../types'

type EnemyProps = {
    enemyType: EnemyType
} & AssetProps

const Enemy: React.FC<EnemyProps> = ({ width, height, x, y, enemyType }) => {
    const handleClick = () => {
        
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={enemyType} onClick={handleClick} />
    )
}

export default Enemy