import React from 'react'
import useLink from '../hooks/useLink'
import { Outlet, useLocation } from 'react-router'
import { PlayerContext } from '../providers/global/PlayerProvider'
import { screenTypeToPageType, screenTypeToURL } from '../utils/page'

const ProperScreenChecker = () => {
    const moveToPage = useLink()
    const location = useLocation()

    const player = React.useContext(PlayerContext)!.player!

    React.useEffect(() => {
        if (screenTypeToURL(player.screenType) != location.pathname) {
            moveToPage(screenTypeToPageType(player.screenType)!)
        }
    }, [player.screenType, moveToPage, location.pathname])

    if (screenTypeToURL(player.screenType) != location.pathname) {
        return null
    }

    return <Outlet />
}

export default ProperScreenChecker