import React from "react"
import type { AssetProps, TileType } from "../../types"
import TileSelector from "./TileSelector"
import { useMutation } from "@tanstack/react-query"
import { updatePlayerPositionMutation, updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import type { ScreenType } from "../../types/api/models/player"
import { useNavigate } from "react-router"

type TileProps = {
    tileType: TileType
    isSelected?: boolean
} & AssetProps

const Tile: React.FC<TileProps> = ({ width, height, x, y, tileType, isSelected = false }) => {
    const navigate = useNavigate();
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(playerId, x, y))

    let screenType: ScreenType
    switch (tileType) {
        case "bank":
            screenType = "Bank"
            break
        case "blacksmith":
            screenType = "Blacksmith"
            break
        case "mine":
            screenType = "Mine"
            break
        case "restaurant":
            screenType = "Restaurant"
            break
        default:
            screenType = "City"
            break
    }

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, screenType))

    const handleClick = async () => {
        switch (tileType) {
            case "bank":
            case "blacksmith":
            case "restaurant":
            case "mine":
                await Promise.all([
                    updatePlayerPositionAsync(),
                    updatePlayerScreenAsync()
                ])
                break
            default:
                await updatePlayerPositionAsync()
                break
        }

        switch (tileType) {
            case "bank":
                navigate("/game/bank")
                break
            case "blacksmith":
                navigate("/game/blacksmith")
                break
            case "restaurant":
                navigate("/game/restaurant")
                break
            case "mine":
                navigate("/game/mine")
                break
        }
    }

    return (
        <>
            <rect x={x} y={y} width={width} height={height} stroke={isSelected ? "red" : "none"} strokeWidth={0.05} />
            <TileSelector width={width} height={height} x={x} y={y} tileType={tileType} onClick={handleClick} />
        </>
    )
}

export default Tile
