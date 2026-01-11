import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { ActiveItemContext } from "../../providers/ActiveItemProvider"
import { useQuery } from "@tanstack/react-query"
import { getPlayerQuery } from "../../api/player"

const FightScreen = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const activeItem = React.useContext(ActiveItemContext)!.activeItemInventoryItemId!

    const player = useQuery(getPlayerQuery(playerId))

    if (player.isLoading) {
        return <div>Loading...</div>
    }

    if (player.isError) {
        return <div>Error loading player data</div>
    }

    if (player.isSuccess) {
        return (
            <div>Fight</div>
        )
    }
}

export default FightScreen
