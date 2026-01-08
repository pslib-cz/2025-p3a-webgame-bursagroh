export type BlockType = "rock" | "wooden_frame" | "copper_ore" | "iron_ore" | "gold_ore" | "silver_ore" | "unobtanium_ore"
export type BuildingType =
    | "grass"
    | "bank"
    | "blacksmith"
    | "fountain"
    | "mine"
    | "restaurant"
    | "road"
    | "road-vertical"
    | "road-horizontal"
    | "abandoned-straight-top"
    | "abandoned-straight-right"
    | "abandoned-straight-bottom"
    | "abandoned-straight-left"
    | "abandoned-trap-straight-top"
    | "abandoned-trap-straight-right"
    | "abandoned-trap-straight-bottom"
    | "abandoned-trap-straight-left"
    | "abandoned-corner-top-left"
    | "abandoned-corner-top-right"
    | "abandoned-corner-bottom-left"
    | "abandoned-corner-bottom-right"
    | "abandoned-trap-corner-top-left"
    | "abandoned-trap-corner-top-right"
    | "abandoned-trap-corner-bottom-left"
    | "abandoned-trap-corner-bottom-right"
export type TileType = BlockType | BuildingType

export type AssetProps = {
    width: number
    height: number
    x: number
    y: number
}
