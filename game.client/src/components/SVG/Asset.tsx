import React from 'react'
import type { AssetType } from '../../types/asset'
import { assetTypeToHref } from '../../utils/asset'

type AssetProps = {
    assetType: AssetType | undefined
    width: number
    height: number
} & Omit<React.SVGProps<SVGUseElement>, 'href' | "shapeRendering" | "width" | "height">

const Asset: React.FC<AssetProps> = ({assetType, width, height, ...props}) => {
    return <use shapeRendering="crispEdges" href={assetTypeToHref(assetType)} width={width + 0.01} height={height + 0.01} {...props} />
}

export default Asset