import type { AssetType } from "../types/asset"

export const assetTypeToId = (assetType: AssetType): string => {
    return `asset_${assetType}`
}

export const assetTypeToHref = (assetType: AssetType): string => {
    return `#${assetTypeToId(assetType)}`
}