import React from 'react'
import { useParams } from 'react-router';
import Layer from '../components/wrappers/layer/Layer';
import { SaveContext } from '../providers/global/SaveProvider';
import { parseSave } from '../utils/save';
import Link from '../components/Link';
import Button from '../components/Button';
import { useMutation } from '@tanstack/react-query';
import { loadMutation } from '../api/save';
import { PlayerIdContext } from '../providers/global/PlayerIdProvider';
import styles from './loadSave.module.css'
import { getPlayerQuery } from '../api/player';
import { queryClient } from '../api';
import useNotification from '../hooks/useNotification';
import useKeyboard from '../hooks/useKeyboard';
import useBlur from '../hooks/useBlur';
import useLink from '../hooks/useLink';
import { screenTypeToPageType } from '../utils/page';

const LoadSaveScreen = () => {
    useBlur(true)

    const moveToPage = useLink()
    const { genericError } = useNotification()
    const saveString = useParams().saveString!

    const { saves, save } = React.useContext(SaveContext)!
    const playerId = React.useContext(PlayerIdContext)!

    const { mutateAsync: loadAsync } = useMutation(loadMutation(playerId.playerId!, saveString, genericError))

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
        if (!playerId.playerId) return

        await save()
        await loadAsync()

        const player = queryClient.getQueryData(getPlayerQuery(playerId.playerId!).queryKey)!
        await moveToPage(screenTypeToPageType(player.screenType))
    }

    const handleJustLoad = async () => {
        if (!playerId.playerId) {
            await playerId.generatePlayerIdAsync()
        }

        await loadAsync()

        const player = queryClient.getQueryData(getPlayerQuery(playerId.playerId!).queryKey)!
        await moveToPage(screenTypeToPageType(player.screenType))
    }

    useKeyboard("Escape", async () => {
        await moveToPage("load")
    })

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
                            <Button onClick={handleSaveAndLoad} disabled={playerId.playerId === null}>Save and Load</Button>
                            <Button onClick={handleJustLoad}>Just Load</Button>
                        </div>
                    </div>
                </div>
                <Link to="load">Back</Link>
            </div>
        </Layer>
    )
}

export default LoadSaveScreen