import React from 'react'
import TileSelector from '../../TileSelector'
import type { AssetProps, EnemyType } from '../../../../types'
import useMove from '../../../../hooks/useMove'
import useLink from '../../../../hooks/useLink'

type EnemyProps = {
    enemyType: EnemyType
} & AssetProps

const Enemy: React.FC<EnemyProps> = ({ width, height, x, y, enemyType }) => {
    const moveToPage = useLink()
    const handleMove = useMove()

    const handleClick = async () => {
        await handleMove(x, y)
        await moveToPage("fight")
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={enemyType} onClick={handleClick} />
    )
}

export default Enemy