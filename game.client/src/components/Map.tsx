import React from 'react'
import styles from './map.module.css'
import CitySVG from './SVG/screen/CitySVG'
import { MapContext } from '../providers/MapProvider'
import MineSVG from './SVG/screen/MineSVG'
import FloorSVG from './SVG/screen/FloorSVG'
import { useMutation } from '@tanstack/react-query'
import { dropItemMutation } from '../api/player'
import { PlayerIdContext } from '../providers/PlayerIdProvider'

const Map: React.FC = () => {
    const mapType = React.useContext(MapContext)!.mapType
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const {mutateAsync: dropItemAsync} = useMutation(dropItemMutation(playerId))

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
                <div className={styles.container} onDrop={handleDrop} onDragOver={(e) => e.preventDefault()}>
                    <FloorSVG />
                </div>
            )
    }
}

export default Map