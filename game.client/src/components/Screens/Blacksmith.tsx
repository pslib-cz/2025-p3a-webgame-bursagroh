import React from 'react'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../../api/player'
import styles from "./blacksmith.module.css"

const BlacksmithScreen = () => {
  const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const handleClick = () => {
        updatePlayerScreenAsync()
    }

    return (
        <div className={styles["blacksmith"]}>
            Blacksmith
            <button onClick={handleClick}>close</button>
        </div>
    )
}

export default BlacksmithScreen