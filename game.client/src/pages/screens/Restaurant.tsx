import React from 'react'
import { useMutation } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"

const RestaurantScreen = () => {
  const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const handleClick = () => {
        updatePlayerScreenAsync()
    }

    return (
        <div>
            Restaurant
            <button onClick={handleClick}>close</button>
        </div>
    )
}

export default RestaurantScreen