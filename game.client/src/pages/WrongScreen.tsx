import React from 'react'
import { NavLink, useNavigate } from 'react-router'
import { screenTypeToURL } from './layouts/Game'
import { PlayerContext } from '../providers/global/PlayerProvider'

const WrongScreen = () => {
    const navigate = useNavigate()

    const player = React.useContext(PlayerContext)!.player!

    React.useEffect(() => {
        navigate(screenTypeToURL(player.screenType)!)
    }, [player.screenType, navigate])
    
    return (
        <>
            <div>Wrong Screen</div>
            <NavLink to={screenTypeToURL(player.screenType)}>change</NavLink>
        </>
    )
}

export default WrongScreen