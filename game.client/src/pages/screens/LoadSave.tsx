import React from 'react'
import { useNavigate, useParams } from 'react-router';
import Layer from '../../components/wrappers/layer/Layer';
import { SaveContext } from '../../providers/SaveProvider';
import { parseSave } from '../../utils/save';
import Link from '../../components/Link';
import Button from '../../components/Button';
import { useMutation } from '@tanstack/react-query';
import { loadMutation } from '../../api/save';
import { PlayerIdContext } from '../../providers/PlayerIdProvider';
import styles from './loadSave.module.css'

const LoadSaveScreen = () => {
    const navigate = useNavigate()
    const saveString = useParams().saveString!

    const { saves, save } = React.useContext(SaveContext)!
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { mutateAsync: loadAsync } = useMutation(loadMutation(playerId, saveString))

    const isAutosave = saves.autosaves.some(save => save.saveString === saveString)
    const isSave = saves.saves.some(save => save.saveString === saveString)

    let displaySaveString = ""
    if (isAutosave) {
        displaySaveString = parseSave(saves.autosaves.find(save => save.saveString === saveString)!, true)
    } else if (isSave) {
        displaySaveString = parseSave(saves.saves.find(save => save.saveString === saveString)!)
    } else {
        displaySaveString = `${saveString} (Not found in saves)`
    }

    const handleSaveAndLoad = async () => {
        await save()
        await loadAsync()
        navigate("/game/city")
    }

    const handleJustLoad = async () => {
        await loadAsync()
        navigate("/game/city")
    }

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <div className={styles.subContainer}>
                    <span className={styles.heading}>Load</span>
                    <div className={styles.saveContainer}>
                        <div className={styles.saveTextContainer}>
                            <span className={styles.saveText}>Loading save:</span>
                            <span className={styles.saveText}>{displaySaveString}</span>
                        </div>
                        <div className={styles.buttonContainer}>
                            <Button onClick={handleSaveAndLoad}>Save and Load</Button>
                            <Button onClick={handleJustLoad}>Just Load</Button>
                        </div>
                    </div>
                </div>
                <Link to="/load">Back</Link>
            </div>
        </Layer>
    )
}

export default LoadSaveScreen