import { useQuery } from "@tanstack/react-query"
import React, { type JSX } from "react"
import { getBuildingsQuery } from "../../api/building"
import type { BuildingType, Building } from "../../types/api/models/building"
import { PlayerIdContext } from "../../providers/global/PlayerIdProvider"
import Asset from "./Asset"
import Road from "./tiles/city/Road"
import BuildingTile from "./tiles/city/Building"
import useNotification from "../../hooks/useNotification"
import { buildingToChunkPosition, mapBuildingTypeToTileType } from "../../utils/map"

type ChunkProps = {
    x: number
    y: number
    size: number
}

const Chunk: React.FC<ChunkProps> = ({ x, y, size }) => {
    const { notify } = useNotification()

    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const buildings = useQuery(getBuildingsQuery(playerId, y, x, size, size))

    const chunkTop = useQuery(getBuildingsQuery(playerId, y - size, x, size, size))
    const chunkRight = useQuery(getBuildingsQuery(playerId, y, x + size, size, size))
    const chunkBottom = useQuery(getBuildingsQuery(playerId, y + size, x, size, size))
    const chunkLeft = useQuery(getBuildingsQuery(playerId, y, x - size, size, size))

     if (buildings.isError) {
        notify("Loading error", `Failed to load chunk x:${x} y:${y}`, 2000)
    }

    if (chunkTop.isError) {
        notify("Loading error", `Failed to load chunk x:${x} y:${y - size}`, 2000)
    }

    if (chunkRight.isError) {
        notify("Loading error", `Failed to load chunk x:${x + size} y:${y}`, 2000)
    }

    if (chunkBottom.isError) {
        notify("Loading error", `Failed to load chunk x:${x} y:${y + size}`, 2000)
    }

    if (chunkLeft.isError) {
        notify("Loading error", `Failed to load chunk x:${x - size} y:${y}`, 2000)
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

                        let text: JSX.Element | null = null
                        if (building.buildingType === "Abandoned" || building.buildingType === "AbandonedTrap") {
                            if (building.isBossDefeated) {
                                text = (
                                    <text x={building.positionX + 0.5} y={building.positionY + 0.5} fontFamily="VT323, monospace" fontSize="0.25" fill="var(--light)" textAnchor="middle" dominantBaseline="central">x</text>
                                )
                            } else if (building.reachedHeight === building.height) {
                                text = (
                                    <text x={building.positionX + 0.5} y={building.positionY + 0.5} fontFamily="VT323, monospace" fontSize="0.25" fill="var(--light)" textAnchor="middle" dominantBaseline="central">{building.height}</text>
                                )
                            } else if (building.reachedHeight !== 0) {
                                text = (
                                    <text x={building.positionX + 0.5} y={building.positionY + 0.5} fontFamily="VT323, monospace" fontSize="0.25" fill="var(--light)" textAnchor="middle" dominantBaseline="central">{building.reachedHeight}/?</text>
                                )
                            }
                        }

                        return (
                            <React.Fragment key={`x:${building.positionX};y:${building.positionY}`}>
                                <BuildingTile width={1} height={1} x={building.positionX} y={building.positionY} buildingType={tileType} />
                                {text}
                            </React.Fragment>
                        )
                    })
                })}
            </>
        )
    }
}

export default Chunk