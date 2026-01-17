import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { Outlet, useLocation } from "react-router"
import { useMutation, useQuery } from "@tanstack/react-query"
import { dropItemMutation, getPlayerInventoryQuery, getPlayerQuery } from "../../api/player"
import type { ScreenType } from "../../types/api/models/player"
import WrongScreen from "../WrongScreen"
import { ActiveItemContext } from "../../providers/ActiveItemProvider"
import { MineIdContext } from "../../providers/MineIdProvider"
import { BuildingIdContext } from "../../providers/BuildingIdProvider"
import { LayerContext } from "../../providers/LayerProvider"

// eslint-disable-next-line react-refresh/only-export-components
export const screenTypeToURL = (screenType: ScreenType, level: number | undefined) => {
    switch (screenType) {
        case "City":
            return "/game/city"
        case "Bank":
            return "/game/bank"
        case "Mine":
            return "/game/mine"
        case "Restaurant":
            return "/game/restaurant"
        case "Blacksmith":
            return "/game/blacksmith"
        case "Floor":
            return `/game/floor/${level}`
        case "Fight":
            return "/game/fight"
    }
}

const ProperScreenChecker = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { data, isError, isPending, isSuccess } = useQuery(getPlayerQuery(playerId))
    const location = useLocation()

    if (isError) {
        return <div>Error</div>
    }

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isSuccess) {
        if (screenTypeToURL(data.screenType, data.floor?.level) != location.pathname) {
            return <WrongScreen />
        }

        return <Outlet />
    }
}

const Inventory = () => {
    const activeItem = React.useContext(ActiveItemContext)!
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const mineId = React.useContext(MineIdContext)!.mineId
    const buildingId = React.useContext(BuildingIdContext)!.buildingId
    const layer = React.useContext(LayerContext)!.layer
    const player = useQuery(getPlayerQuery(playerId))
    const inventory = useQuery(getPlayerInventoryQuery(playerId))

    const {mutateAsync: dropItemAsync} = useMutation(dropItemMutation(playerId, mineId ?? -1, buildingId ?? -1, layer ?? -1))

    const handleActiveItemChange = (inventoryItemId: number) => {
        activeItem.setActiveItemInventoryItemId(prev => prev === inventoryItemId ? null : inventoryItemId)
    }

    const handleDropItem = async (inventoryItemId: number) => {
        await dropItemAsync(inventoryItemId)
    }

    if (player.isError || inventory.isError) {
        return <div>Error</div>
    }

    if (player.isPending || inventory.isPending) {
        return <div>Loading...</div>
    }

    if (player.isSuccess && inventory.isSuccess) {
        return (
            <>
                <div>Inventory for player {player.data.playerId}</div>
                <div>Money: {player.data.money}</div>
                {inventory.data.map((item) => (
                    <div key={item.inventoryItemId}>
                        Item: {item.itemInstance.item.name}
                        <button onClick={() => handleActiveItemChange(item.inventoryItemId)}>
                            {activeItem.activeItemInventoryItemId === item.inventoryItemId ? "Deactivate" : "Set as Active"}
                        </button>
                        <button onClick={() => handleDropItem(item.inventoryItemId)}>drop</button>
                    </div>
                ))}
            </>
        )
    }
}

const Game = () => {
    const { playerId } = React.useContext(PlayerIdContext)!

    if (playerId === null) {
        return <div>Loading...</div>
    }

    return (
        <>
            <div>Player ID: {playerId}</div>
            <ProperScreenChecker />
            <Inventory />
        </>
    )
}

export default Game
