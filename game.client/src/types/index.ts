export type BlockType = "rock"
export type BuildingType = "bank" | "blacksmith" | "fountain" | "mine" | "restaurant" | "road"
export type TileType = BlockType | BuildingType

export type AssetProps = {
    width: number
    height: number
    x: number
    y: number
}