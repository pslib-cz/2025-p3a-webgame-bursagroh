import React from "react"
import { MineIdContext } from "../../providers/MineIdProvider"
import { useQuery } from "@tanstack/react-query"
import { getMineLayersQuery } from "../../api/mine"
import Tile from "./Tile"
import type { BlockType } from "../../types/api/models/mine"
import type { TileType } from "../../types"

const mapBlockTypeToTileType = (buildingType: BlockType): TileType => {
    switch (buildingType) {
        case "Wooden_Frame":
            return "wooden_frame"
        case "Rock":
            return "rock"
        case "Copper_Ore":
            return "copper_ore"
        case "Iron_Ore":
            return "iron_ore"
        case "Gold_Ore":
            return "gold_ore"
        case "Silver_Ore":
            return "silver_ore"
        case "Unobtanium_Ore":
            return "unobtanium_ore"
        default:
            return "rock"
    }
}

type LayerProps = {
    depth: number
    size: number
}

const Layer: React.FC<LayerProps> = ({ depth, size }) => {
    const mineId = React.useContext(MineIdContext)!.mineId!
    const { data, isError, isPending, isSuccess } = useQuery(getMineLayersQuery(mineId, depth, depth + size - 1))

    if (isError) {
        return <div>Error loading.</div>
    }

    if (isPending) {
        return <div>Loading layer...</div>
    }

    if (isSuccess) {
        return (
            <>
                {data.map((layer) => layer.mineBlocks.map((block) => (
                    <Tile key={`${layer.depth}-${block.index}`} width={1} height={1} x={block.index} y={layer.depth} tileType={mapBlockTypeToTileType(block.block.blockType)} />
                )))}
            </>
        )
    }
}

export default Layer
