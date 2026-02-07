import React from 'react'
import type { AssetProps } from '../../../../types'
import { useNavigate } from 'react-router'
import { PlayerContext } from '../../../../providers/game/PlayerProvider'
import { getPlayerQuery, updatePlayerPositionMutation } from '../../../../api/player'
import { validMove } from '../../../../utils/player'
import TileSelector from '../../TileSelector'
import { useMutation } from '@tanstack/react-query'
import { queryClient } from '../../../../api'
import useNotification from '../../../../hooks/useNotification'

type BuildingProps = {
    buildingType: | "bank"
    | "blacksmith"
    | "fountain"
    | "mine"
    | "restaurant"
    | "abandoned-straight-top"
    | "abandoned-straight-right"
    | "abandoned-straight-bottom"
    | "abandoned-straight-left"
    | "abandoned-trap-straight-top"
    | "abandoned-trap-straight-right"
    | "abandoned-trap-straight-bottom"
    | "abandoned-trap-straight-left"
    | "abandoned-corner-top-left"
    | "abandoned-corner-top-right"
    | "abandoned-corner-bottom-left"
    | "abandoned-corner-bottom-right"
    | "abandoned-trap-corner-top-left"
    | "abandoned-trap-corner-top-right"
    | "abandoned-trap-corner-bottom-left"
    | "abandoned-trap-corner-bottom-right"
} & AssetProps

const Building: React.FC<BuildingProps> = ({x, y, width, height, buildingType}) => {
    const navigate = useNavigate()
    const notify = useNotification()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(player.playerId, x, y))

    const handleClick = async () => {
        if (!validMove(player.positionX, player.positionY, x, y)) {
            notify("Error", "You cannot move that far.", 1000)
            return
        }

        await updatePlayerPositionAsync()

        switch (buildingType) {
            case 'bank':
                navigate("/game/bank")
                break
            case 'blacksmith':
                navigate("/game/blacksmith")
                break
            case 'fountain':
                navigate("/game/fountain")
                break
            case 'mine':
                await queryClient.refetchQueries({ queryKey: [player.playerId, "player"] })
                await queryClient.fetchQuery(getPlayerQuery(player.playerId))
                navigate("/game/mine")
                break
            case 'restaurant':
                navigate("/game/restaurant")
                break
            case 'abandoned-straight-top':
            case 'abandoned-straight-right':
            case 'abandoned-straight-bottom':
            case 'abandoned-straight-left':
            case 'abandoned-trap-straight-top':
            case 'abandoned-trap-straight-right':
            case 'abandoned-trap-straight-bottom':
            case 'abandoned-trap-straight-left':
            case 'abandoned-corner-top-left':
            case 'abandoned-corner-top-right':
            case 'abandoned-corner-bottom-left':
            case 'abandoned-corner-bottom-right':
            case 'abandoned-trap-corner-top-left':
            case 'abandoned-trap-corner-top-right':
            case 'abandoned-trap-corner-bottom-left':
            case 'abandoned-trap-corner-bottom-right':
                navigate("/game/floor")
                break
        }
    }

    return (
        <TileSelector width={width} height={height} x={x} y={y} tileType={buildingType} onClick={handleClick} />
    )
}

export default Building