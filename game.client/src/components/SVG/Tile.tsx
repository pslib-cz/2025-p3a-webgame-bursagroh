import React from "react"
import type { AssetProps, TileType } from "../../types"
import TileSelector from "./TileSelector"
import { useMutation } from "@tanstack/react-query"
import { updatePlayerPositionMutation, updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import type { ScreenType } from "../../types/api/models/player"

type TileProps = {
    tileType: TileType
    isSelected?: boolean
} & AssetProps

const Tile: React.FC<TileProps> = ({ width, height, x, y, tileType, isSelected = false}) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(playerId, x, y))

    let screenType: ScreenType
    switch (tileType) {
        case "bank":
            screenType = "Bank"
            break
        case "blacksmith":
            screenType = "Bank"
            break
        case "mine":
            screenType = "Bank"
            break
        case "restaurant":
            screenType = "Bank"
            break
        default:
            screenType = "City"
            break
    }

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, screenType))

    const handleClick = async () => {
        await updatePlayerPositionAsync()

        switch (tileType) {
            case "bank":
            case "blacksmith":
            case "mine":
            case "restaurant":
                updatePlayerScreenAsync()
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
