import React from 'react'
import { PlayerIdContext } from '../providers/PlayerIdProvider'
import { useQuery } from '@tanstack/react-query'
import { getPlayerQuery } from '../api/player'
import { NavLink } from 'react-router'
import { screenTypeToURL } from './layouts/Game'

const WrongScreen = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { data, isError, isPending, isSuccess } = useQuery(getPlayerQuery(playerId))

     if (isError) {
        return <div>Error</div>
    }

    if (isPending) {
        return <div>Loading...</div>
    }

    if (isSuccess) {
        return (
            <>
                <div>Wrong Screen</div>
                <NavLink to={screenTypeToURL(data.screenType)}>change</NavLink>
            </>
        )
    }
}

export default WrongScreen