import React from 'react'
import { dropItemMutation, updatePlayerScreenMutation } from '../../api/player'
import { useMutation } from '@tanstack/react-query'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useNavigate } from 'react-router'
import CloseIcon from '../../assets/icons/CloseIcon'
import styles from './fountain.module.css'
import useBlur from '../../hooks/useBlur'

const FountainScreen = () => {
    useBlur(true)
    
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))
    const {mutateAsync: dropItemAsync} = useMutation(dropItemMutation(playerId))

    const handleClick = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault()
        const inventoryItemId = Number(event.dataTransfer.getData("text/plain"))
        dropItemAsync(inventoryItemId)
    }

    return (
        <div className={styles.container}>
            <div className={styles.fountainContainer}>
                <div className={styles.header}>
                    <span className={styles.heading}>Fountain of Sacrifice</span>
                    <CloseIcon width={24} height={24} className={styles.close} onClick={handleClick} />
                </div>
                <div className={styles.transferContainer} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                    <span className={styles.transferText}>Throw here the mythical sword</span>
                </div>
            </div>
        </div>
    )
}

export default FountainScreen