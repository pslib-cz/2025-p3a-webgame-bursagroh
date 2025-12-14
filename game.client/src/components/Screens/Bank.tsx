import { useMutation } from "@tanstack/react-query"
import styles from "./bank.module.css"
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
        <div className={styles["bank"]}>
            Bank
            <button onClick={handleClick}>close</button>
        </div>
    )
}

export default BankScreen
