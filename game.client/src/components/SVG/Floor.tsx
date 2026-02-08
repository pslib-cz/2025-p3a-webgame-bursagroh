import { useQuery } from '@tanstack/react-query'
import FloorTile from './tiles/floor/FloorTile'
import { getBuildingsQuery } from '../../api/building'
import React from 'react'
import { getChunkCoords } from '../../utils/map'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { buildingToChunkPosition, mapBuildingTypeToTileType } from './Chunk'
import type { Building, BuildingType } from '../../types/api/models/building'

const mapPositionToTileType = (x: number, y: number, buildingType: "fountain" | "bank" | "restaurant" | "mine" | "blacksmith" | "abandoned-corner-bottom-left" | "abandoned-corner-top-left" | "abandoned-corner-top-right" | "abandoned-corner-bottom-right" | "abandoned-straight-bottom" | "abandoned-straight-left" | "abandoned-straight-top" | "abandoned-straight-right" | "abandoned-trap-corner-bottom-left" | "abandoned-trap-corner-top-left" | "abandoned-trap-corner-top-right" | "abandoned-trap-corner-bottom-right" | "abandoned-trap-straight-bottom" | "abandoned-trap-straight-left" | "abandoned-trap-straight-top" | "abandoned-trap-straight-right" | "road" | "road-vertical" | "road-horizontal", isGroundFloor: boolean) => {
    if (x === 0 && y === 0) return "wall-top-left"
    if (x === 0 && y === 7) return "wall-bottom-left"
    if (x === 7 && y === 0) return "wall-top-right"
    if (x === 7 && y === 7) return "wall-bottom-right"
    if (y === 0) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-top-left" || buildingType === "abandoned-corner-top-right" || buildingType === "abandoned-straight-top" || buildingType === "abandoned-trap-corner-top-left" || buildingType === "abandoned-trap-corner-top-right" || buildingType === "abandoned-trap-straight-top") {
                if (x === 3) {
                    return "wall-door-right-top"
                } else if (x === 4) {
                    return "wall-door-left-top"
                }
            }
        }
        return "wall-top"
    }
    if (y === 7) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-bottom-left" || buildingType === "abandoned-corner-bottom-right" || buildingType === "abandoned-straight-bottom" || buildingType === "abandoned-trap-corner-bottom-left" || buildingType === "abandoned-trap-corner-bottom-right" || buildingType === "abandoned-trap-straight-bottom") {
                if (x === 3) {
                    return "wall-door-left-bottom"
                } else if (x === 4) {
                    return "wall-door-right-bottom"
                }
            }
        }
        return "wall-bottom"
    }
    if (x === 0) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-top-left" || buildingType === "abandoned-corner-bottom-left" || buildingType === "abandoned-straight-left" || buildingType === "abandoned-trap-corner-top-left" || buildingType === "abandoned-trap-corner-bottom-left" || buildingType === "abandoned-trap-straight-left") {
                if (y === 3) {
                    return "wall-door-left-left"
                } else if (y === 4) {
                    return "wall-door-right-left"
                }
            }
        }
        return "wall-left"
    }
    if (x === 7) {
        if (isGroundFloor) {
            if (buildingType === "abandoned-corner-top-right" || buildingType === "abandoned-corner-bottom-right" || buildingType === "abandoned-straight-right" || buildingType === "abandoned-trap-corner-top-right" || buildingType === "abandoned-trap-corner-bottom-right" || buildingType === "abandoned-trap-straight-right") {
                if (y === 3) {
                    return "wall-door-right-right"
                } else if (y === 4) {
                    return "wall-door-left-right"
                }
            }
        }
        return "wall-right"
    }
    return "floor"
}

type FloorProps = {
    positionX: number
    positionY: number
    level: number
}

const CHUNK_SIZE = 16

const Floor: React.FC<FloorProps> = ({ positionX, positionY, level }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const chunkCoordinates = getChunkCoords(positionX, positionY, CHUNK_SIZE)
    const x = chunkCoordinates.x
    const y = chunkCoordinates.y
    const size = CHUNK_SIZE

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

        const playerRelativeX = positionX - x
        const playerRelativeY = positionY - y

        const building = chunkBuildingMap[playerRelativeY][playerRelativeX]!

        const tileType = mapBuildingTypeToTileType(
            building.buildingType,
            buildingsMap[playerRelativeY][playerRelativeX + 1],
            buildingsMap[playerRelativeY + 1][playerRelativeX + 2],
            buildingsMap[playerRelativeY + 2][playerRelativeX + 1],
            buildingsMap[playerRelativeY + 1][playerRelativeX]
        )

        return (
            <>
                {new Array(8).fill(null).flatMap((_, y) => new Array(8).fill(null).map((_, x) => {
                    return <FloorTile key={`x:${x};y:${y}`} x={x} y={y} width={1} height={1} floorTileType={mapPositionToTileType(x, y, tileType, level === 0)} />
                }))}
            </>
        )
    }
}

export default Floor