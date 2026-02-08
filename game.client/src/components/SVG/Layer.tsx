import React from "react"
import { useQuery } from "@tanstack/react-query"
import { getMineLayersQuery } from "../../api/mine"
import type { MineLayer } from "../../types/api/models/mine"
import { PlayerIdContext } from "../../providers/global/PlayerIdProvider"
import Block from "./tiles/mine/Block"
import MineTile from "./tiles/mine/MineTile"
import useNotification from "../../hooks/useNotification"
import { mapBlockTypeToTileType } from "../../utils/mine"

type LayerProps = {
    depth: number
    size: number
    mineId: number
}

const Layer: React.FC<LayerProps> = ({ depth, size, mineId }) => {
    const {notify} = useNotification()

    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { data, isSuccess, isError } = useQuery(getMineLayersQuery(playerId, mineId, depth, depth + size - 1))

    if (isError) {
        notify("Loading error", `Failed to load mine layers ${depth} to ${depth + size - 1}`, 2000)
    }

    if (isSuccess) {
        return (
            <>
                {data.map((layer) => {
                    const layerMap: Array<MineLayer["mineBlocks"][0] | null> = Array(size).fill(null)

                    layer.mineBlocks.forEach((block) => {
                        layerMap[block.index] = block
                    })

                    return layerMap.map((block, index) => {
                        if (!block) {
                            return <MineTile key={`${layer.depth}-${index}`} width={1} height={1} x={index} y={layer.depth} mineTileType="empty" />
                        }

                        return (
                            <Block key={`${layer.depth}-${index}`} width={1} height={1} x={index} y={layer.depth} blockType={mapBlockTypeToTileType(block.block.blockType)} health={block.health} maxHealth={5} />
                        )
                    })
                })}
            </>
        )
    }
}

export default Layer
