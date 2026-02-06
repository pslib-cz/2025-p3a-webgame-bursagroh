import React from 'react'
import Layer from '../../components/wrappers/layer/Layer'
import Link from '../../components/Link'
import Input from '../../components/Input'
import { SaveContext } from '../../providers/SaveProvider'
import { parseSave } from '../../utils/save'
import styles from './load.module.css'
import SendIcon from '../../assets/icons/SendIcon'

const LoadScreen = () => {
    const [userSaveString, setUserSaveString] = React.useState("")

    const saves = React.useContext(SaveContext)!.saves

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <div className={styles.subContainer}>
                    <span className={styles.heading}>Load</span>
                    <div className={styles.savesContainer}>
                        {saves.autosaves.map((save, index) => (
                            <Link key={`autosave_${index}`} to={`/load/${encodeURIComponent(save.saveString)}`}>{parseSave(save, true)}</Link>
                        ))}
                        {saves.saves.map((save, index) => (
                            <Link key={`save_${index}`} to={`/load/${encodeURIComponent(save.saveString)}`}>{parseSave(save)}</Link>
                        ))}
                        <div className={styles.inputContainer}>
                            <Input placeholder="SaveString" value={userSaveString} onChange={e => setUserSaveString(e.target.value)} />
                            <Link to={`/load/${encodeURIComponent(userSaveString)}`}>
                                <SendIcon className={styles.send} width={48} height={48} />
                            </Link>
                        </div>
                    </div>
                </div>
                <Link to="/">Back</Link>
            </div>
        </Layer>
    )
}

export default LoadScreen