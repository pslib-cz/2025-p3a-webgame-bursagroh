import React, { useState, useEffect } from 'react'
import HomeIcon from '../icons/HomeIcon'
import styles from './navbar.module.css'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { SaveContext } from '../providers/global/SaveProvider'
import SaveString from './SaveString'
import SaveIcon from '../icons/SaveIcon'
import useLink from '../hooks/useLink'

const NavBar = () => {
    const moveToPage = useLink()
    const player = React.useContext(PlayerContext)!.player!
    const { save, saveState, saveString } = React.useContext(SaveContext)!
    const [showIcon, setShowIcon] = useState(true)

    useEffect(() => {
        if (saveState === "saving") {
            setShowIcon(false)
        }
    }, [saveState])

    const handleSaveFinished = () => {
        setShowIcon(true)
    }

    const handleClick = async () => {
        await moveToPage("root")
    }

    return (
        <div className={styles.container}>
            <div className={styles.homeContainer}>
                <HomeIcon className={styles.home} width={64} height={64} onClick={handleClick} />
            </div>
            
            <span className={styles.location}>{player.screenType}</span>
            
            <div className={styles.savingContainer}>
                {saveState === "idle" && showIcon && (
                    <SaveIcon className={styles.save} width={64} height={64} onClick={() => save()} />
                )}

                {saveState === "saving" && <span className={styles.saveText}>Saving...</span>}

                {saveState === "saved" && (
                    <SaveString 
                        saveString={saveString!} 
                        onFinished={handleSaveFinished} 
                    />
                )}
            </div>
        </div>
    )
}

export default NavBar