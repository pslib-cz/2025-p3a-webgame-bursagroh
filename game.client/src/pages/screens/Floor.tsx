import React from 'react'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation, useQuery } from '@tanstack/react-query'
import { getPlayerQuery, updatePlayerScreenMutation } from '../../api/player'
import { useParams } from 'react-router'
import type { FloorPathParams } from '../../types'
import { getBuildingFloorQuery, getBuildingsQuery } from '../../api/building'
import type { Player } from '../../types/api/models/player'

const BuildingFetch = ({playerId, player, level}: {playerId: string, player: Player, level: number}) => {
    const {data, isPending, isError, isSuccess} = useQuery(getBuildingsQuery(playerId, player.positionY, player.positionX, 1, 1))

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isError) {
        return <div>Error</div>
    }

    if (isSuccess) {
        return <Floor playerId={playerId} buildingId={data[0].buildingId} level={level} />
    }
}

const Floor = ({playerId, buildingId, level}: {playerId: string, buildingId: number, level: number}) => {
    const {data, isPending, isError, isSuccess} = useQuery(getBuildingFloorQuery(playerId, buildingId, level))

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

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
        return <BuildingFetch playerId={playerId} player={data} level={Number(params.level)} />
    }
}

export default FloorScreen