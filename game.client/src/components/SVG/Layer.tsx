import React from "react"
import { useQuery } from "@tanstack/react-query"
import { getMineLayersQuery } from "../../api/mine"
import type { BlockType, MineLayer } from "../../types/api/models/mine"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import Block from "./tiles/mine/Block"

const mapBlockTypeToTileType = (buildingType: BlockType) => {
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
            return "unobtainium_ore"
    }
}

type LayerProps = {
    depth: number
    size: number
    mineId: number
}

const Layer: React.FC<LayerProps> = ({ depth, size, mineId }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { data, isError, isPending, isSuccess } = useQuery(getMineLayersQuery(playerId, mineId, depth, depth + size - 1))

    if (isError) {
        return <div>Error loading.</div>
    }

    if (isPending) {
        return <div>Loading layer...</div>
    }

    if (isSuccess) {
        return (
            <>
                {data.map((layer) => {
                    const layerMap: Array<MineLayer["mineBlocks"][0] | null> = Array(size).fill(null)

                    layer.mineBlocks.forEach((block) => {
                        layerMap[block.index] = block
                    })

                    return layerMap.map((block, index) => (
                        <Block key={`${layer.depth}-${index}`} width={1} height={1} x={index} y={layer.depth} blockType={mapBlockTypeToTileType(block!.block.blockType)} />
                    ))
                })}
            </>
        )
    }
}

export default Layer
