import React from 'react'
import Layer from '../components/wrappers/layer/Layer'
import { SaveContext } from '../providers/global/SaveProvider'
import Link from '../components/Link'
import SaveString from '../components/SaveString'
import styles from './save.module.css'
import useKeyboard from '../hooks/useKeyboard'
import { PlayerIdContext } from '../providers/global/PlayerIdProvider'
import useBlur from '../hooks/useBlur'
import useLink from '../hooks/useLink'
import Text from '../components/Text'

const SaveScreen = () => {
    useBlur(true)

    const moveToPage = useLink()
    
    const saveString = React.useContext(SaveContext)!.saveString!
    const playerId = React.useContext(PlayerIdContext)!.playerId

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })

    React.useEffect(() => {
        if (!playerId) {
            moveToPage("root")
        }
    }, [playerId, moveToPage])

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <div className={styles.subContainer}>
                    <Text size='h1'>Save</Text>
                    <SaveString saveString={saveString} />
                </div>
                <Link to='root'>Back</Link>
            </div>
        </Layer>
    )
}

export default SaveScreen