import React from 'react'
import HomeIcon from '../assets/icons/HomeIcon'
import styles from './navbar.module.css'
import { PlayerContext } from '../providers/game/PlayerProvider'
import { useNavigate } from 'react-router'
import { SaveContext } from '../providers/SaveProvider'
import SaveString from './SaveString'
import SaveIcon from '../assets/icons/SaveIcon'

const NavBar = () => {
    const navigate = useNavigate()

    const player = React.useContext(PlayerContext)!.player!
    const {save, saveState, saveString} = React.useContext(SaveContext)!

    const handleClick = () => {
        navigate("/")
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