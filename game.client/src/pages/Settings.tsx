import React from 'react'
import useBlur from '../hooks/useBlur'
import Layer from '../components/wrappers/layer/Layer'
import Button from '../components/Button'
import Link from '../components/Link'
import Input from '../components/Input'
import styles from './settings.module.css'
import { DEFAULT_AUTOSAVE_INTERVAL, DEFAULT_MAX_AUTOSAVE } from '../constants/settings'
import { SettingsContext } from '../providers/global/SettingsProvider'
import useKeyboard from '../hooks/useKeyboard'
import useLink from '../hooks/useLink'
import Text from '../components/Text'

const SettingsScreen = () => {
    useBlur(true)

    const moveToPage = useLink()

    const autosave = React.useContext(SettingsContext)!

    const [autosaveInterval, setAutosaveInterval] = React.useState(autosave.autosaveInterval)
    const [maxAutosave, setMaxAutosave] = React.useState(autosave.maxAutosave)

    const handleSave = () => {
        autosave.setAutosaveInterval(autosaveInterval)
        autosave.setMaxAutosave(maxAutosave)
    }

    const handleDelete = () => {
        autosave.deleteAutosaveInterval()
        autosave.deleteMaxAutosave()
        setAutosaveInterval(DEFAULT_AUTOSAVE_INTERVAL)
        setMaxAutosave(DEFAULT_MAX_AUTOSAVE)
    }

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })
    
    return (
        <Layer layer={1} >
            <div className={styles.container}>
                <Text size="h1" className={styles.heading}>Settings</Text>
                <div className={styles.settingsContainer}>
                    <Text size="h3" className={styles.settingsText}>Autosave interval</Text>
                    <Input type="number" value={autosaveInterval} onChange={e => setAutosaveInterval(Number(e.target.value))} />
                    <Text size="h3" className={styles.settingsText}>Max autosave</Text>
                    <Input type="number" value={maxAutosave} onChange={e => setMaxAutosave(Number(e.target.value))} />
                    <Button disabled={autosaveInterval === autosave.autosaveInterval && maxAutosave === autosave.maxAutosave} onClick={handleSave}>Save</Button>
                </div>
                <div className={styles.buttonContainer}>
                    <Button isDangerous onClick={handleDelete}>Delete Stored Info</Button>
                    <Link to="root">Back</Link>
                </div>
            </div>
        </Layer>
    )
}

export default SettingsScreen