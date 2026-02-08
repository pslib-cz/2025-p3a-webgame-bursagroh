import React from 'react'
import HomeIcon from '../assets/icons/HomeIcon'
import styles from './navbar.module.css'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { SaveContext } from '../providers/global/SaveProvider'
import SaveString from './SaveString'
import SaveIcon from '../assets/icons/SaveIcon'
import useLink from '../hooks/useLink'

const NavBar = () => {
    const moveToPage = useLink()

    const player = React.useContext(PlayerContext)!.player!
    const {save, saveState, saveString} = React.useContext(SaveContext)!

    const handleClick = async () => {
        await moveToPage("root")
    }

    return (
        <div className={styles.container}>
            <HomeIcon className={styles.home} width={64} height={64} onClick={handleClick} />
            <span className={styles.location}>{player.screenType}</span>
            {saveState === "idle" && <SaveIcon className={styles.save} width={64} height={64} onClick={() => save()} />}
            {saveState === "saving" && <span className={styles.saveText}>Saving...</span>}
            {saveState === "saved" && <SaveString saveString={saveString!} />}
        </div>
    )
}

export default NavBar