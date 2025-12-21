import React from "react"
import SVGDisplay from "../SVGDisplay"
import Tile from "../SVG/Tile"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useQuery } from "@tanstack/react-query"
import { getBuildingsQuery } from "../../api/building"
import type { BuildingType } from "../../types/api/models/building"
import type { TileType } from "../../types"
import { getPlayerQuery } from "../../api/player"
import Player from "../../assets/Player"

const mapBuildingTypeToTileType = (buildingType: BuildingType): TileType => {
    switch (buildingType) {
        case "Fountain":
            return "fountain"
        case "Bank":
            return "bank"
        case "Restaurant":
            return "restaurant"
        case "Mine":
            return "mine"
        case "Blacksmith":
            return "blacksmith"
        default:
            return "rock"
    }
}

const CityScreen = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const buildings = useQuery(getBuildingsQuery(playerId))
    const player = useQuery(getPlayerQuery(playerId))

    if (buildings.isError || player.isError) {
        return <div>Error loading.</div>
    }

    if (buildings.isPending || player.isPending) {
        return <div>Loading...</div>
    }

    if (buildings.isSuccess && player.isSuccess) {
        return (
            <SVGDisplay width={"99vw"} height={"99vh"}>
                {buildings.data.map((building) => (
                    <Tile width={1} height={1} x={building.positionX} y={building.positionY} tileType={mapBuildingTypeToTileType(building.buildingType)} />
                ))}
                <Player x={player.data.floorItem.positionX} y={player.data.floorItem.positionY} width={1} height={1} />
            </SVGDisplay>
        )
    }
}

export default CityScreen
