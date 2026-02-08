import React from 'react'
import type { AssetProps } from "../../../../types/asset"
import useMove from '../../../../hooks/useMove'
import useLink from '../../../../hooks/useLink'
import Asset from '../../Asset'
import type { EnemyType } from '../../../../types/enemy'

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
        <Asset width={width} height={height} x={x} y={y} assetType={enemyType} onClick={handleClick} />
    )
}

export default Enemy