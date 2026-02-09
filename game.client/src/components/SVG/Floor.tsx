import { useQuery } from '@tanstack/react-query'
import FloorTile from './tiles/floor/FloorTile'
import { getBuildingsQuery } from '../../api/building'
import React from 'react'
import { getChunkCoords } from '../../utils/map'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { buildingToChunkPosition } from "../../utils/map"
import { mapBuildingTypeToTileType } from "../../utils/map"
import type { Building, BuildingType } from '../../types/api/models/building'
import useNotification from '../../hooks/useNotification'
import { CHUNK_SIZE } from '../../constants/map'
import { mapPositionToTileType } from '../../utils/floor'

type FloorProps = {
    positionX: number
    positionY: number
    level: number
}

const Floor: React.FC<FloorProps> = ({ positionX, positionY, level }) => {
    const {notify} = useNotification()

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