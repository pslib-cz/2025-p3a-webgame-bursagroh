import React from 'react'
import Asset from '../../Asset'
import type { AssetProps } from '../../../../types'
import useLink from '../../../../hooks/useLink'

const Minecard: React.FC<AssetProps> = ({x, y, width, height}) => {
    const moveToPage = useLink()

    const handleLeave = async () => {        
        await moveToPage("city", true)
    }

    return (
        <Asset assetType='minecard' x={x} y={y} width={width} height={height} onClick={handleLeave} />
    )
}

export default Minecard