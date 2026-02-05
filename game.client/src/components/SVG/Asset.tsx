import React from 'react'
import type { AssetType } from '../../types/asset'
import { assetTypeToHref } from '../../utils/asset'

type AssetProps = {
    assetType: AssetType | undefined
} & Omit<React.SVGProps<SVGUseElement>, 'href'>

const Asset: React.FC<AssetProps> = ({assetType, ...props}) => {
    return <use href={assetTypeToHref(assetType)} {...props} />
}

export default Asset