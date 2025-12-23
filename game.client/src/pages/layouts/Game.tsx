import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { Outlet, useLocation } from "react-router"
import { useQuery } from "@tanstack/react-query"
import { getPlayerQuery } from "../../api/player"
import type { ScreenType } from "../../types/api/models/player"
import WrongScreen from "../WrongScreen"

// eslint-disable-next-line react-refresh/only-export-components
export const screenTypeToURL = (screenType: ScreenType) => {
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
            return "/game/floor"
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
        if (screenTypeToURL(data.screenType) != location.pathname) {
            return <WrongScreen />
        }

        return <Outlet />
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
        </>
    )
}

export default Game
