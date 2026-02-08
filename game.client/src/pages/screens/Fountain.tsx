import React from 'react'
import { dropItemMutation, updatePlayerScreenMutation } from '../../api/player'
import { useMutation } from '@tanstack/react-query'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { useNavigate } from 'react-router'
import CloseIcon from '../../assets/icons/CloseIcon'
import styles from './fountain.module.css'
import useBlur from '../../hooks/useBlur'
import useNotification from '../../hooks/useNotification'
import useKeyboard from '../../hooks/useKeyboard'

const FountainScreen = () => {
    useBlur(true)

    const navigate = useNavigate()
    const {genericError} = useNotification()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City", genericError))
    const { mutateAsync: dropItemAsync } = useMutation(dropItemMutation(playerId, genericError))

    const handleEscape = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault()
        const data = event.dataTransfer.getData("text/plain")
        if (data.startsWith("inv_")) {
            const inventoryItemId = Number(data.substring(4))
            dropItemAsync(inventoryItemId)
        } else if (data.startsWith("hand_")) {
            const inventoryItemId = Number(data.substring(5))
            dropItemAsync(inventoryItemId)
        }
    }

    useKeyboard("Escape", handleEscape)

    return (
        <div className={styles.container}>
            <div className={styles.legendContainer}>
                <span className={styles.heading}>Legend</span>
                <div className={styles.innerLegendContainer}>
                    <span className={styles.legendText}>Once, a purple mist turned almost everyone into monsters. Only a few humans are left. Legend says that if a hero throws a Mythical Sword into the Magic Fountain, everyone will turn back into humans and the world will be saved.</span>
                </div>
            </div>
            <div className={styles.fountainContainer}>
                <div className={styles.header}>
                    <span className={styles.heading}>Fountain of Sacrifice</span>
                    <CloseIcon width={24} height={24} className={styles.close} onClick={handleEscape} />
                </div>
                <div className={styles.transferContainer} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                    <span className={styles.transferText}>Throw here the mythical sword</span>
                </div>
            </div>
            <div className={styles.tutorialContainer}>
                <span className={styles.heading}>How to Play</span>
                <span className={styles.heading}>Survival Rules</span>
                <ol className={styles.innerTutorialContainer}>
                    <li>1. The Restaurant: Go here first. Work and serve the remaining humans to earn Cash.</li>
                    <li>2. The Mines: Use your cash to rent a pickaxe and dig for Resources. You need these to craft better tools at the blacksmith.</li>
                    <li>3. The Monster Buildings: Enter dangerous buildings to fight monsters. You will find a lot of valuable loot there.</li>
                    <li>4. The Fountain: Once the sword is ready, take it to the fountain to win the game!</li>
                </ol>
                <ul className={styles.innerTutorialContainer}>
                    <li>- Death Costs Everything: If you die, you lose your entire inventory. Store your items and money in the Bank to keep them safe.</li>
                    <li>- Red Buildings (Traps): Red buildings are traps. Once you enter, the doors lock. You must defeat the Boss to be able to leave.</li>
                    <li>- Crafting: Collect enough rare materials to forge the sword. Don't rush into big fights without good weapons and some potions!</li>
                </ul>
            </div>
        </div>
    )
}

export default FountainScreen