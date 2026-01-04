export type BlockType = "rock" | "wooden_frame" | "copper_ore" | "iron_ore" | "gold_ore" | "silver_ore" | "unobtanium_ore"
export type BuildingType = "bank" | "blacksmith" | "fountain" | "mine" | "restaurant" | "road" | "road-vertical" | "road-horizontal" | "abandoned" | "abandoned-trap"
export type TileType = BlockType | BuildingType

export type AssetProps = {
    width: number
    height: number
    x: number
    y: number
}