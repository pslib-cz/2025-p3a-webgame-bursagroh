import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getBuildingsQuery } from "../../api/building"
import type { BuildingType, Building } from "../../types/api/models/building"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import Asset from "./Asset"
import Road from "./tiles/city/Road"
import BuildingTile from "./tiles/city/Building"

const mapBuildingTypeToTileType = (buildingType: BuildingType, buildingTypeTop: BuildingType | null, buildingTypeRight: BuildingType | null, buildingTypeBottom: BuildingType | null, buildingTypeLeft: BuildingType | null) => {
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
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-corner-bottom-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-corner-top-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-corner-top-right"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-corner-bottom-right"

            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeBottom === "Road") return "abandoned-straight-bottom"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && buildingTypeLeft === "Road") return "abandoned-straight-left"
            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeTop === "Road") return "abandoned-straight-top"
            return "abandoned-straight-right"
        case "AbandonedTrap":
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-trap-corner-bottom-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap")) return "abandoned-trap-corner-top-left"
            if ((buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-trap-corner-top-right"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap")) return "abandoned-trap-corner-bottom-right"

            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeBottom === "Road")
                return "abandoned-trap-straight-bottom"
            if ((buildingTypeTop === "Abandoned" || buildingTypeTop === "AbandonedTrap") && (buildingTypeBottom === "Abandoned" || buildingTypeBottom === "AbandonedTrap") && buildingTypeLeft === "Road") return "abandoned-trap-straight-left"
            if ((buildingTypeLeft === "Abandoned" || buildingTypeLeft === "AbandonedTrap") && (buildingTypeRight === "Abandoned" || buildingTypeRight === "AbandonedTrap") && buildingTypeTop === "Road") return "abandoned-trap-straight-top"
            return "abandoned-trap-straight-right"
        case "Road":
            if (buildingTypeTop === "Road" && buildingTypeRight === "Road" && buildingTypeBottom === "Road" && buildingTypeLeft === "Road") return "road"
            if (buildingTypeTop === "Road" && buildingTypeBottom === "Road") return "road-vertical"
            return "road-horizontal"
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
        y: ((building.positionY % chunkSize) + chunkSize) % chunkSize,
    }
}

type ChunkProps = {
    x: number
    y: number
    size: number
}

const Chunk: React.FC<ChunkProps> = ({ x, y, size }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const buildings = useQuery(getBuildingsQuery(playerId, y, x, size, size))

    const chunkTop = useQuery(getBuildingsQuery(playerId, y - size, x, size, size))
    const chunkRight = useQuery(getBuildingsQuery(playerId, y, x + size, size, size))
    const chunkBottom = useQuery(getBuildingsQuery(playerId, y + size, x, size, size))
    const chunkLeft = useQuery(getBuildingsQuery(playerId, y, x - size, size, size))

    if (buildings.isError || chunkTop.isError || chunkRight.isError || chunkBottom.isError || chunkLeft.isError) {
        return <div>Error loading.</div>
    }

    if (buildings.isPending || chunkTop.isPending || chunkRight.isPending || chunkBottom.isPending || chunkLeft.isPending) {
        return <div>Loading...</div>
    }

    if (buildings.isSuccess && chunkTop.isSuccess && chunkRight.isSuccess && chunkBottom.isSuccess && chunkLeft.isSuccess) {
        const chunkTopMap: Array<Array<BuildingType | null>> = [...Array(size)].map(() => Array(size).fill(null))
        const chunkRightMap: Array<Array<BuildingType | null>> = [...Array(size)].map(() => Array(size).fill(null))
        const chunkBottomMap: Array<Array<BuildingType | null>> = [...Array(size)].map(() => Array(size).fill(null))
        const chunkLeftMap: Array<Array<BuildingType | null>> = [...Array(size)].map(() => Array(size).fill(null))

        chunkTop.data.forEach((building) => {
            const position = buildingToChunkPosition(building, size)
            chunkTopMap[position.y][position.x] = building.buildingType
        })

        chunkRight.data.forEach((building) => {
            const position = buildingToChunkPosition(building, size)
            chunkRightMap[position.y][position.x] = building.buildingType
        })

        chunkBottom.data.forEach((building) => {
            const position = buildingToChunkPosition(building, size)
            chunkBottomMap[position.y][position.x] = building.buildingType
        })

        chunkLeft.data.forEach((building) => {
            const position = buildingToChunkPosition(building, size)
            chunkLeftMap[position.y][position.x] = building.buildingType
        })

        const mapSize = size + 2
        const buildingsMap: Array<Array<BuildingType | null>> = [...Array(mapSize)].map(() => Array(mapSize).fill(null))
        const chunkBuildingMap: Array<Array<Building | null>> = [...Array(size)].map(() => Array(size).fill(null))

        buildings.data.forEach((building) => {
            const position = buildingToChunkPosition(building, size)

            chunkBuildingMap[position.y][position.x] = building
            buildingsMap[position.y + 1][position.x + 1] = building.buildingType
        })

        buildingsMap.forEach((row, rowIndex) => {
            row.forEach((_, columnIndex) => {
                if (rowIndex === 0 && columnIndex !== 0 && columnIndex !== mapSize - 1) {
                    buildingsMap[rowIndex][columnIndex] = chunkTopMap[size - 1][columnIndex - 1]
                } else if (columnIndex === mapSize - 1 && rowIndex !== 0 && rowIndex !== mapSize - 1) {
                    buildingsMap[rowIndex][columnIndex] = chunkRightMap[rowIndex - 1][0]
                } else if (rowIndex === mapSize - 1 && columnIndex !== 0 && columnIndex !== mapSize - 1) {
                    buildingsMap[rowIndex][columnIndex] = chunkBottomMap[0][columnIndex - 1]
                } else if (columnIndex === 0 && rowIndex !== 0 && rowIndex !== mapSize - 1) {
                    buildingsMap[rowIndex][columnIndex] = chunkLeftMap[rowIndex - 1][size - 1]
                }
            })
        })

        return (
            <>
                {chunkBuildingMap.map((row, y_index) => {
                    return row.map((building, x_index) => {
                        if (building === null) {
                            const positionX = x + x_index
                            const positionY = y + y_index

                            return (
                                <Asset key={`x:${positionX};y:${positionY}`} width={1} height={1} x={positionX} y={positionY} assetType="grass" />
                            )
                        }

                        const tileType = mapBuildingTypeToTileType(
                            building.buildingType,
                            buildingsMap[y_index][x_index + 1],
                            buildingsMap[y_index + 1][x_index + 2],
                            buildingsMap[y_index + 2][x_index + 1],
                            buildingsMap[y_index + 1][x_index]
                        )

                        if (tileType === "road" || tileType === "road-horizontal" || tileType === "road-vertical") {
                            return (
                                <Road key={`x:${building.positionX};y:${building.positionY}`} width={1} height={1} x={building.positionX} y={building.positionY} roadType={tileType} />
                            )
                        }

                        return (
                            <BuildingTile key={`x:${building.positionX};y:${building.positionY}`} width={1} height={1} x={building.positionX} y={building.positionY} buildingType={tileType} />
                        )
                    })
                })}
            </>
        )
    }
}

export default Chunk