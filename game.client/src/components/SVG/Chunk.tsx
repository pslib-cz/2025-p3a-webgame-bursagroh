import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getBuildingsQuery } from "../../api/building"
import Tile from "./Tile"
import type { Building, BuildingType } from "../../types/api/models/building"
import type { TileType } from "../../types"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"

const mapBuildingTypeToTileType = (
    buildingType: BuildingType,
    buildingTypeTop: BuildingType | null,
    buildingTypeRight: BuildingType | null,
    buildingTypeBottom: BuildingType | null,
    buildingTypeLeft: BuildingType | null
): TileType => {
    switch (buildingType) {
        case "Fountain":
            return "fountain"
        case "Bank":
            return "bank"
        case "Restaurant":
            return "restaurant"
        case "Mine":
            return "mine"
        case "Blacksmith":
            return "blacksmith"
        case "Abandoned":
            return "abandoned"
        case "AbandonedTrap":
            return "abandoned-trap"
        case "Road":
            if (buildingTypeTop === "Road" && buildingTypeRight === "Road" && buildingTypeBottom === "Road" && buildingTypeLeft === "Road") return "road"
            if (buildingTypeTop === "Road" && buildingTypeBottom === "Road") return "road-vertical"
            return "road-horizontal"
        default:
            return "rock"
    }
}

const buildingToChunkPosition = (
    building: Building,
    chunkSize: number
): {
    x: number
    y: number
} => {
    return {
        x: ((building.positionX % chunkSize) + chunkSize) % chunkSize,
        y: ((building.positionY % chunkSize) + chunkSize) % chunkSize
    }
}

type ChunkProps = {
    x: number
    y: number
    size: number
}

const Chunk: React.FC<ChunkProps> = ({ x, y, size }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const buildings = useQuery(getBuildingsQuery(playerId, x, y, size - 1, size - 1))

    if (buildings.isError) {
        return <div>Error loading.</div>
    }

    if (buildings.isPending) {
        return <div>Loading...</div>
    }

    if (buildings.isSuccess) {
        const buildingsMap: Array<Array<BuildingType | null>> = new Array(size + 2).fill(new Array(size + 2).fill(null))

        buildings.data.forEach((building) => {
            const position = buildingToChunkPosition(building, size)

            buildingsMap[position.y + 1][position.x + 1] = building.buildingType
        })

        return (
            <>
                {buildings.data.map((building) => {
                    const position = buildingToChunkPosition(building, size)

                    return (
                        <Tile
                            key={`x:${building.positionX};y:${building.positionY}`}
                            width={1}
                            height={1}
                            x={building.positionX}
                            y={building.positionY}
                            tileType={mapBuildingTypeToTileType(
                                building.buildingType,
                                buildingsMap[position.y - 1 + 1][position.x + 1],
                                buildingsMap[position.y + 1][position.x + 1 + 1],
                                buildingsMap[position.y + 1 + 1][position.x + 1],
                                buildingsMap[position.y + 1][position.x - 1 + 1]
                            )}
                        />
                    )
                })}
            </>
        )
    }
}

export default Chunk
