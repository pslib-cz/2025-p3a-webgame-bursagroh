import React from 'react'
import useLock from './useLock'
import { useNavigate } from 'react-router'
import { IsBluredContext } from '../providers/global/IsBluredProvider'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../api/player'
import { PlayerIdContext } from '../providers/global/PlayerIdProvider'
import useNotification from './useNotification'
import { MapContext } from '../providers/global/MapProvider'
import type { PageType } from '../types/page'
import { pageTypeToScreenType, pageTypeToURL } from '../utils/page'

const useLink = () => {
    const handleLock = useLock()
    const navigate = useNavigate()
    const { genericError } = useNotification()

    const playerId = React.useContext(PlayerIdContext)!.playerId
    const setIsBlured = React.useContext(IsBluredContext)!.setIsBlured
    const setMapType = React.useContext(MapContext)!.setMapType

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(genericError))

    const moveToPage = async (page: PageType, moveScreen?: boolean, saveString?: string) => {
        await handleLock(async () => {
            switch (page) {
                case 'bank':
                case 'blacksmith':
                case 'fight':
                case 'fountain':
                case 'lose':
                case 'restaurant':
                case 'win':
                case 'load':
                case 'save':
                case 'settings':
                case 'root':
                case 'loadSave':
                    setIsBlured(true)
                    break
                case 'city':
                case 'floor':
                case 'mine':
                    setIsBlured(false)
                    break
            }

            switch (page) {
                case 'city':
                    setMapType("city")
                    break
                case 'floor':
                    setMapType("floor")
                    break
                case 'mine':
                    setMapType("mine")
                    break
            }

            if (moveScreen && playerId) {
                await updatePlayerScreenAsync({ playerId, newScreenType: pageTypeToScreenType(page) })
            }

            await navigate(pageTypeToURL(page, saveString))
        })
    }

    return moveToPage
}

export default useLink