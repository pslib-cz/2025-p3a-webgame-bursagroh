import React from 'react'
import { useMutation } from "@tanstack/react-query"
import styles from "./restaurant.module.css"
import { updatePlayerScreenMutation } from "../../api/player"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"

const RestaurantScreen = () => {
  const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const handleClick = () => {
        updatePlayerScreenAsync()
    }

    return (
        <div className={styles["restaurant"]}>
            Restaurant
            <button onClick={handleClick}>close</button>
        </div>
    )
}

export default RestaurantScreen