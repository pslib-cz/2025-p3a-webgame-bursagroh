import React from 'react'
import HomeIcon from '../assets/icons/HomeIcon'
import { PlayerIdContext } from '../providers/PlayerIdProvider'
import { getPlayerQuery } from '../api/player'
import { useQuery } from '@tanstack/react-query'
import styles from './navbar.module.css'

const NavBar = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = useQuery(getPlayerQuery(playerId))

    if (player.isError) {
        return <div>Error</div>
    }

    if (player.isPending) {
        return <div>Loading...</div>
    }

    if (player.isSuccess) {
        return (
            <div className={styles.container}>
                <HomeIcon className={styles.home} width={64} height={64} />
                <h2 className={styles.location}>{player.data.screenType}</h2>
                <span />
            </div>
        )
    }
}

export default NavBar