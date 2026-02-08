import React from 'react'
import styles from './map.module.css'
import CitySVG from './SVG/screen/CitySVG'
import { MapContext } from '../providers/global/MapProvider'
import MineSVG from './SVG/screen/MineSVG'
import FloorSVG from './SVG/screen/FloorSVG'
import { useMutation } from '@tanstack/react-query'
import { dropItemMutation } from '../api/player'
import { PlayerIdContext } from '../providers/global/PlayerIdProvider'
import useNotification from '../hooks/useNotification'
import ProviderGroupLoadingWrapper from './wrappers/ProviderGroupLoadingWrapper'
import FloorProvider, { FloorContext } from '../providers/game/FloorProvider'
import type { TLoadingWrapperContextState } from './wrappers/LoadingWrapper'

const Map: React.FC = () => {
    const {genericError} = useNotification()
    
    const mapType = React.useContext(MapContext)!.mapType
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const {mutateAsync: dropItemAsync} = useMutation(dropItemMutation(playerId, genericError))

    const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
        event.preventDefault()
        const inventoryItemId = Number(event.dataTransfer.getData("text/plain"))
        dropItemAsync(inventoryItemId)
    }

    switch (mapType) {
        case 'city':
            return (
                <div className={styles.container}>
                    <CitySVG />
                </div>
            )
        case 'mine':
            return (
                <div className={styles.container} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                    <MineSVG />
                </div>
            )
        case 'floor':
            return (
                <ProviderGroupLoadingWrapper providers={[FloorProvider]} contextsToLoad={[FloorContext] as Array<React.Context<TLoadingWrapperContextState>>}>
                    <div className={styles.container} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                        <FloorSVG />
                    </div>
                </ProviderGroupLoadingWrapper>
            )
    }
}

export default Map