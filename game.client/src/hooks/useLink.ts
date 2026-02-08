import React from 'react'
import type { ScreenType } from '../types/api/models/player'
import useLock from './useLock'
import { useNavigate } from 'react-router'
import { IsBluredContext } from '../providers/global/IsBluredProvider'
import { useMutation } from '@tanstack/react-query'
import { updatePlayerScreenMutation } from '../api/player'
import { PlayerIdContext } from '../providers/global/PlayerIdProvider'
import useNotification from './useNotification'
import { MapContext } from '../providers/global/MapProvider'

export type PageType = "bank" | "blacksmith" | "city" | "fight" | "floor" | "fountain" | "lose" | "mine" | "restaurant" | "win" | "load" | "save" | "settings" | "root" | "loadSave"

export const pageTypeToURL = (pageType: PageType, saveString?: string) => {
    switch (pageType) {
        case "bank":
            return "/game/bank"
        case "blacksmith":
            return "/game/blacksmith"
        case "city":
            return "/game/city"
        case "fight":
            return "/game/fight"
        case "floor":
            return "/game/floor"
        case "fountain":
            return "/game/fountain"
        case "lose":
            return "/game/lose"
        case "mine":
            return "/game/mine"
        case "restaurant":
            return "/game/restaurant"
        case "win":
            return "/game/win"
        case "load":
            return "/load"
        case "save":
            return "/save"
        case "settings":
            return "/settings"
        case "root":
            return "/"
        case "loadSave":
            return `/loadSave/${encodeURIComponent(saveString!)}`
    }
}

export const screenTypeToPageType = (screenType: ScreenType): PageType => {
    switch (screenType) {
        case "City":
            return "city"
        case "Bank":
            return "bank"
        case "Mine":
            return "mine"
        case "Restaurant":
            return "restaurant"
        case "Blacksmith":
            return "blacksmith"
        case "Floor":
            return "floor"
        case "Fight":
            return "fight"
        case "Fountain":
            return "fountain"
        case "Win":
            return "win"
        case "Lose":
            return "lose"
    }
}

export const pageTypeToScreenType = (pageType: Omit<PageType, "load" | "save" | "settings" | "root" | "loadSave">): ScreenType => {
    switch (pageType) {
        case "city":
            return "City"
        case "bank":
            return "Bank"
        case "mine":
            return "Mine"
        case "restaurant":
            return "Restaurant"
        case "blacksmith":
            return "Blacksmith"
        case "floor":
            return "Floor"
        case "fight":
            return "Fight"
        case "fountain":
            return "Fountain"
        case "win":
            return "Win"
        case "lose":
            return "Lose"
    }

    return undefined as never
}

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