import React from 'react'
import { dropItemMutation } from '../../api/player'
import { useMutation } from '@tanstack/react-query'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import CloseIcon from '../../icons/CloseIcon'
import styles from './fountain.module.css'
import useBlur from '../../hooks/useBlur'
import useNotification from '../../hooks/useNotification'
import useKeyboard from '../../hooks/useKeyboard'
import useLink from '../../hooks/useLink'
import Text from '../../components/Text'

const FountainScreen = () => {
    useBlur(true)

    const moveToPage = useLink()
    const {genericError} = useNotification()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { mutateAsync: dropItemAsync } = useMutation(dropItemMutation(playerId, genericError))

    const handleEscape = async () => {
        await moveToPage("city", true)
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
                <Text size="h3">Legend</Text>
                <div className={styles.innerLegendContainer}>
                    <Text size="h5">Once, a purple mist turned almost everyone into monsters. Only a few humans are left. Legend says that if a hero throws a Mythical Sword into the Magic Fountain, everyone will turn back into humans and the world will be saved.</Text>
                </div>
            </div>
            <div className={styles.fountainContainer}>
                <div className={styles.header}>
                    <Text size="h3">Fountain of Sacrifice</Text>
                    <CloseIcon className={styles.close} onClick={handleEscape} />
                </div>
                <div className={styles.transferContainer} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                    <Text size="h4">Throw here the mythical sword</Text>
                </div>
            </div>
            <div className={styles.tutorialContainer}>
                <Text size="h3">How to Play</Text>
                <Text size="h3">Survival Rules</Text>
                <div className={styles.innerTutorialContainer}>
                    <Text size="h5">1. The Restaurant: Go here first. Work and serve the remaining humans to earn Cash.</Text>
                    <Text size="h5">2. The Mines: Use your cash to rent a pickaxe and dig for Resources. You need these to craft better tools at the blacksmith.</Text>
                    <Text size="h5">3. The Monster Buildings: Enter dangerous buildings to fight monsters. You will find a lot of valuable loot there.</Text>
                    <Text size="h5">4. The Fountain: Once the sword is ready, take it to the fountain to win the game!</Text>
                </div>
                <div className={styles.innerTutorialContainer}>
                    <Text size="h5">- Death Costs Everything: If you die, you lose your entire inventory. Store your items and money in the Bank to keep them safe.</Text>
                    <Text size="h5">- Red Buildings (Traps): Red buildings are traps. Once you enter, the doors lock. You must defeat the Boss to be able to leave.</Text>
                    <Text size="h5">- Crafting: Collect enough rare materials to forge the sword. Don't rush into big fights without good weapons and some potions!</Text>
                </div>
            </div>
        </div>
    )
}

export default FountainScreen