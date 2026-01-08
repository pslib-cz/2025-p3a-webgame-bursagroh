import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { Outlet, useLocation } from "react-router"
import { useQuery } from "@tanstack/react-query"
import { getPlayerInventoryQuery, getPlayerQuery } from "../../api/player"
import type { ScreenType } from "../../types/api/models/player"
import WrongScreen from "../WrongScreen"
import { ActivePickaxeContext } from "../../providers/ActivePickaxeProvider"

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
    const activePickaxe = React.useContext(ActivePickaxeContext)!
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = useQuery(getPlayerQuery(playerId))
    const inventory = useQuery(getPlayerInventoryQuery(playerId))

    const handleActivePickaxeChange = (inventoryItemId: number) => {
        activePickaxe.setActivePickaxeInventoryItemId(prev => prev === inventoryItemId ? null : inventoryItemId)
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
                        <button onClick={() => handleActivePickaxeChange(item.inventoryItemId)}>
                            {activePickaxe.activePickaxeInventoryItemId === item.inventoryItemId ? "Deactivate Pickaxe" : "Set as Active Pickaxe"}
                        </button>
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
