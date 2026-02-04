import React from 'react'
import useBlur from '../../hooks/useBlur'
import Layer from '../../components/wrappers/layer/Layer'
import Button from '../../components/Button'
import Link from '../../components/Link'
import Input from '../../components/Input'
import styles from './settings.module.css'
import { DEFAULT_AUTOSAVE_INTERVAL, DEFAULT_MAX_AUTOSAVE } from '../../constants/settings'
import { AutosaveContext } from '../../providers/AutosaveProvider'

const SettingsScreen = () => {
    useBlur(true)

    const autosave = React.useContext(AutosaveContext)!

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
    
    return (
        <Layer layer={1} >
            <div className={styles.container}>
                <span className={styles.heading}>Settings</span>
                <div className={styles.settingsContainer}>
                    <span className={styles.settingsText}>Autosave interval</span>
                    <Input type="number" min={1} max={10} value={autosaveInterval} onChange={e => setAutosaveInterval(Number(e.target.value))} />
                    <span className={styles.settingsText}>Max autosave</span>
                    <Input type="number" min={0} max={10} value={maxAutosave} onChange={e => setMaxAutosave(Number(e.target.value))} />
                    <Button disabled={autosaveInterval === autosave.autosaveInterval && maxAutosave === autosave.maxAutosave} onClick={handleSave}>Save</Button>
                </div>
                <div className={styles.buttonContainer}>
                    <Button isDangerous onClick={handleDelete}>Delete Stored Info</Button>
                    <Link to="/">Back</Link>
                </div>
            </div>
        </Layer>
    )
}

export default SettingsScreen