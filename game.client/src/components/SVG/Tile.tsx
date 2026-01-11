import React from "react"
import type { AssetProps, TileType } from "../../types"
import TileSelector from "./TileSelector"
import { useMutation } from "@tanstack/react-query"
import { updatePlayerFloorMutation, updatePlayerPositionMutation, updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import type { ScreenType } from "../../types/api/models/player"
import { useNavigate } from "react-router"
import { ActivePickaxeContext } from "../../providers/ActivePickaxeProvider"
import { mineMineBlockMutation } from "../../api/mine"

type TileProps = {
    tileType: TileType
    targetFloorId?: number
    targetLevel?: number
    mineId?: number
} & AssetProps

const Tile: React.FC<TileProps> = ({ width, height, x, y, tileType, targetFloorId: targetFloorId, targetLevel, mineId }) => {
    const navigate = useNavigate();
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const activePickaxe = React.useContext(ActivePickaxeContext)!

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
        case "abandoned-straight-top":
        case "abandoned-straight-right":
        case "abandoned-straight-bottom":
        case "abandoned-straight-left":
        case "abandoned-trap-straight-top":
        case "abandoned-trap-straight-right":
        case "abandoned-trap-straight-bottom":
        case "abandoned-trap-straight-left":
        case "abandoned-corner-top-left":
        case "abandoned-corner-top-right":
        case "abandoned-corner-bottom-left":
        case "abandoned-corner-bottom-right":
        case "abandoned-trap-corner-top-left":
        case "abandoned-trap-corner-top-right":
        case "abandoned-trap-corner-bottom-left":
        case "abandoned-trap-corner-bottom-right":
            screenType = "Floor"
            break
        case "floor":
        case "wall-top":
        case "wall-right":
        case "wall-bottom":
        case "wall-left":
        case "wall-top-left":
        case "wall-top-right":
        case "wall-bottom-left":
        case "wall-bottom-right":
        case "stair":
        case "zombie":
        case "skeleton":
        case "dragon":
            screenType = "Floor"
            break
        case "empty":
        case "rock":
        case "wooden_frame":
        case "copper_ore":
        case "iron_ore":
        case "gold_ore":
        case "silver_ore":
        case "unobtanium_ore":
            screenType = "Mine"
            break
        case "grass":
        case "fountain":
        case "road":
        case "road-vertical":
        case "road-horizontal":
        case "wall-door-left-top":
        case "wall-door-left-right":
        case "wall-door-left-bottom":
        case "wall-door-left-left":
        case "wall-door-right-top":
        case "wall-door-right-right":
        case "wall-door-right-bottom":
        case "wall-door-right-left":
            screenType = "City"
            break
    }

    const { mutateAsync: updatePlayerPositionAsync } = useMutation(updatePlayerPositionMutation(playerId, x, y))
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, screenType))
    const { mutateAsync: updatePlayerFloorAsync } = useMutation(updatePlayerFloorMutation(playerId, x, y, targetFloorId!))
    const { mutateAsync: mineMineBlockAsync } = useMutation(mineMineBlockMutation(playerId, mineId ?? -1, activePickaxe.activePickaxeInventoryItemId ?? -1, x, y))

    const handleClick = async () => {
        switch (tileType) {
            case "bank":
            case "blacksmith":
            case "restaurant":
            case "mine":
            case "wall-door-left-top":
            case "wall-door-left-right":
            case "wall-door-left-bottom":
            case "wall-door-left-left":
            case "wall-door-right-top":
            case "wall-door-right-right":
            case "wall-door-right-bottom":
            case "wall-door-right-left":
            case "abandoned-straight-top":
            case "abandoned-straight-right":
            case "abandoned-straight-bottom":
            case "abandoned-straight-left":
            case "abandoned-trap-straight-top":
            case "abandoned-trap-straight-right":
            case "abandoned-trap-straight-bottom":
            case "abandoned-trap-straight-left":
            case "abandoned-corner-top-left":
            case "abandoned-corner-top-right":
            case "abandoned-corner-bottom-left":
            case "abandoned-corner-bottom-right":
            case "abandoned-trap-corner-top-left":
            case "abandoned-trap-corner-top-right":
            case "abandoned-trap-corner-bottom-left":
            case "abandoned-trap-corner-bottom-right":
                await Promise.all([
                    updatePlayerPositionAsync(),
                    updatePlayerScreenAsync()
                ])
                break
            case "stair":
                await updatePlayerPositionAsync()
                await updatePlayerFloorAsync()
                break
            case "rock":
            case "wooden_frame":
            case "copper_ore":
            case "iron_ore":
            case "gold_ore":
            case "silver_ore":
            case "unobtanium_ore":
                await mineMineBlockAsync()
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
            case "wall-door-left-top":
            case "wall-door-left-right":
            case "wall-door-left-bottom":
            case "wall-door-left-left":
            case "wall-door-right-top":
            case "wall-door-right-right":
            case "wall-door-right-bottom":
            case "wall-door-right-left":
                navigate("/game/city")
                break
            case "abandoned-straight-top":
            case "abandoned-straight-right":
            case "abandoned-straight-bottom":
            case "abandoned-straight-left":
            case "abandoned-trap-straight-top":
            case "abandoned-trap-straight-right":
            case "abandoned-trap-straight-bottom":
            case "abandoned-trap-straight-left":
            case "abandoned-corner-top-left":
            case "abandoned-corner-top-right":
            case "abandoned-corner-bottom-left":
            case "abandoned-corner-bottom-right":
            case "abandoned-trap-corner-top-left":
            case "abandoned-trap-corner-top-right":
            case "abandoned-trap-corner-bottom-left":
            case "abandoned-trap-corner-bottom-right":
                navigate("/game/floor/0")
                break
            case "stair":
                navigate(`/game/floor/${targetLevel}`)
                break
        }
    }

    return (
        <>
            <TileSelector width={width} height={height} x={x} y={y} tileType={tileType} onClick={handleClick} />
        </>
    )
}

export default Tile
