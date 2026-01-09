import React from 'react'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation, useQuery } from '@tanstack/react-query'
import { getPlayerQuery, updatePlayerScreenMutation } from '../../api/player'
import { useParams } from 'react-router'
import type { FloorPathParams } from '../../types'
import { getBuildingFloorQuery, getBuildingsQuery } from '../../api/building'
import type { Player as PlayerType } from '../../types/api/models/player'
import SVGDisplay from '../../components/SVGDisplay'
import Player from '../../assets/Player'
import FloorSVG from '../../components/SVG/Floor'
import Tile from '../../components/SVG/Tile'
import type { Building } from '../../types/api/models/building'

const BuildingFetch = ({player, level}: {player: PlayerType, level: number}) => {
    const {data, isPending, isError, isSuccess} = useQuery(getBuildingsQuery(player.playerId, player.positionY, player.positionX, 1, 1))

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isError) {
        return <div>Error</div>
    }

    if (isSuccess) {
        return <Floor player={player} building={data[0]} level={level} />
    }
}

const Floor = ({player, building, level}: {player: PlayerType, building: Building, level: number}) => {
    const {data, isPending, isError, isSuccess} = useQuery(getBuildingFloorQuery(player.playerId, building.buildingId, level))

    const floorAbove = useQuery(getBuildingFloorQuery(player.playerId, building.buildingId, (building.buildingType === "Abandoned" || building.buildingType === "AbandonedTrap") && building.height === level ? level : level + 1))
    const floorBelow = useQuery(getBuildingFloorQuery(player.playerId, building.buildingId, level === 0 ? 0 : level - 1))

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(player.playerId, "City"))

    const handleClick = () => {
        updatePlayerScreenAsync()
    }

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isError) {
        return <div>Error</div>
    }

    if (isSuccess) {
        return (
            <div>
                <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.subPositionX} centerY={player.subPositionY}>
                    <FloorSVG />
                    {data.floorItems.map((item) => {
                        if (item.floorItemType === "Stair") {
                            const targetLevel = (level ?? 0) + (item.positionX > 3 ? 1 : -1)
                            const targetFloorId = item.positionX > 3 ? floorAbove.data?.floorId : floorBelow.data?.floorId

                            return (
                                <Tile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} tileType='stair' targetLevel={targetLevel} targetFloorId={targetFloorId} />
                            )
                        }
                    })}
                    <Player x={player.subPositionX} y={player.subPositionY} width={1} height={1} />
                </SVGDisplay>
                Floor - {data.level}
                <button onClick={handleClick}>close</button>
            </div>
        )
    }
}

const FloorScreen = () => {
    const params = useParams<FloorPathParams>()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const {data, isPending, isError, isSuccess} = useQuery(getPlayerQuery(playerId))

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isError) {
        return <div>Error</div>
    }

    if (isSuccess) {
        return <BuildingFetch player={data} level={Number(params.level)} />
    }
}

export default FloorScreen