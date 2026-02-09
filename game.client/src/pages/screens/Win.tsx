import React from 'react'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import useBlur from '../../hooks/useBlur'
import styles from './win.module.css'
import useKeyboard from '../../hooks/useKeyboard'
import Button from '../../components/Button'
import useLink from '../../hooks/useLink'
import Text from '../../components/Text'

const WinScreen = () => {
    useBlur(true)

    const moveToPage = useLink()

    const playerId = React.useContext(PlayerIdContext)!

    const handleClick = async () => {
        await playerId.generatePlayerIdAsync()
        await moveToPage("fountain")
    }

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })

    return (
        <div className={styles.container}>
            <Text size="h0">You Win</Text>
            <Button onClick={handleClick}>New Game</Button>
        </div>
    )
}

export default WinScreen