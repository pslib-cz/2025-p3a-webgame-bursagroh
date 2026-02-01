import React from 'react'
import HomeIcon from '../assets/icons/HomeIcon'
import styles from './navbar.module.css'
import { PlayerContext } from '../providers/game/PlayerProvider'
import { useNavigate } from 'react-router'

const NavBar = () => {
    const navigate = useNavigate()
    const player = React.useContext(PlayerContext)!.player!

    const handleClick = () => {
        navigate("/")
    }

    return (
        <div className={styles.container}>
            <HomeIcon className={styles.home} width={64} height={64} onClick={handleClick} />
            <span className={styles.location}>{player.screenType}</span>
            <span />
        </div>
    )
}

export default NavBar