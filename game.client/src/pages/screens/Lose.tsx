import useBlur from '../../hooks/useBlur'
import styles from './lose.module.css'
import Button from '../../components/Button'
import useKeyboard from '../../hooks/useKeyboard'
import useLink from '../../hooks/useLink'
import Text from '../../components/Text'

const LoseScreen = () => {
    useBlur(true)

    const moveToPage = useLink()

    const handleClick = async () => {
        await moveToPage("city", true)
    }

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })

    return (
        <div className={styles.container}>
            <Text size="h0">You Died</Text>
            <Button onClick={handleClick}>Respawn</Button>
        </div>
    )
}

export default LoseScreen