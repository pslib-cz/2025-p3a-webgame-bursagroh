import React from 'react'
import Layer from '../components/wrappers/layer/Layer'
import Link from '../components/Link'
import Input from '../components/Input'
import { SaveContext } from '../providers/global/SaveProvider'
import { parseSave } from '../utils/save'
import styles from './load.module.css'
import SendIcon from '../icons/SendIcon'
import useKeyboard from '../hooks/useKeyboard'
import useBlur from '../hooks/useBlur'
import useLink from '../hooks/useLink'

const LoadScreen = () => {
    useBlur(true)

    const moveToPage = useLink()

    const [userSaveString, setUserSaveString] = React.useState("")

    const saves = React.useContext(SaveContext)!.saves

    const handleLoad = async () => {
        await moveToPage("loadSave", false, userSaveString)
    }

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <div className={styles.subContainer}>
                    <span className={styles.heading}>Load</span>
                    <div className={styles.savesContainer}>
                        <div className={styles.savesLinkContainer}>
                            {saves.autosaves.map((save, index) => (
                                <Link key={`autosave_${index}`} to="loadSave" saveString={save.saveString}>{parseSave(save, true)}</Link>
                            ))}
                            {saves.saves.map((save, index) => (
                                <Link key={`save_${index}`} to="loadSave" saveString={save.saveString}>{parseSave(save)}</Link>
                            ))}
                        </div>
                        <div className={styles.inputContainer}>
                            <Input placeholder="SaveString" value={userSaveString} onChange={e => setUserSaveString(e.target.value)} />
                            <SendIcon className={styles.send} width={48} height={48} onClick={handleLoad} />
                        </div>
                    </div>
                </div>
                <Link to="root">Back</Link>
            </div>
        </Layer>
    )
}

export default LoadScreen