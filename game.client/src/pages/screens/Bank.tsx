import { useMutation } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"

const BankScreen = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const handleClick = () => {
        updatePlayerScreenAsync()
    }

    return (
        <div>
            Bank
            <button onClick={handleClick}>close</button>
        </div>
    )
}

export default BankScreen
