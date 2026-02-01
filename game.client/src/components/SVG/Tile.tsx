import React from "react"
import type { AssetProps, TileType } from "../../types"
import TileSelector from "./TileSelector"
import { useMutation } from "@tanstack/react-query"
import { pickItemMutation, updatePlayerFloorMutation, updatePlayerPositionMutation, updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import type { ScreenType } from "../../types/api/models/player"
import { useNavigate } from "react-router"
import { ActiveItemContext } from "../../providers/ActiveItemProvider"
import { mineMineBlockMutation } from "../../api/mine"
import { interactInBuildingMutation } from "../../api/building"

type TileProps = {
    tileType: TileType
    targetFloorId?: number
    targetFloorItemId?: number
    targetLevel?: number
    targetBuildingId?: number
    mineId?: number
} & AssetProps

const Tile: React.FC<TileProps> = ({ width, height, x, y, tileType, targetFloorId, targetLevel, targetBuildingId, mineId, targetFloorItemId }) => {
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const activeItem = React.useContext(ActiveItemContext)!

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
        case "zombie":
        case "skeleton":
        case "dragon":
        case "stair":
            screenType = "Floor"
            break
        case "empty":
        case "rock":
        case "wooden_frame":
        case "copper_ore":
        case "iron_ore":
        case "gold_ore":
        case "silver_ore":
        case "unobtainium_ore":
        case "wooden_sword":
        case "wooden_pickaxe":
        case "wood":
        case "rock_item":
        case "copper":
        case "iron":
        case "silver":
        case "gold":
        case "unobtainium":
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
    const { mutateAsync: mineMineBlockAsync } = useMutation(mineMineBlockMutation(playerId, mineId ?? -1, activeItem.activeItemInventoryItemId ?? -1, x, y))
    const {mutateAsync: interactInBuildingAsync} = useMutation(interactInBuildingMutation(playerId, targetBuildingId ?? -1, targetLevel ?? -1, activeItem.activeItemInventoryItemId ?? -1, x, y))
    const {mutateAsync: pickItemAsync} = useMutation(pickItemMutation(playerId, mineId ?? -1, targetBuildingId ?? -1, targetLevel ?? -1))

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
                await updatePlayerPositionAsync()
                // await updatePlayerScreenAsync()
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
            case "unobtainium_ore":
                await mineMineBlockAsync()
                break
            case "zombie":
            case "skeleton":
            case "dragon":
                await interactInBuildingAsync()
                break
            case "wooden_sword":
            case "wooden_pickaxe":
            case "wood":
            case "rock_item":
            case "copper":
            case "iron":
            case "silver":
            case "gold":
            case "unobtainium":
                await pickItemAsync(targetFloorItemId ?? -1)
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
            case "fountain":
                navigate("/game/fountain")
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
                navigate("/game/floor")
                break
            case "stair":
                navigate("/game/floor")
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
