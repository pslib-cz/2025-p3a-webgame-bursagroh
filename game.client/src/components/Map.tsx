import React from 'react'
import styles from './map.module.css'
import CitySVG from './SVG/screen/CitySVG'
import { MapContext } from '../providers/MapProvider'
import MineSVG from './SVG/screen/MineSVG'
import FloorSVG from './SVG/screen/FloorSVG'

const Map: React.FC = () => {
    const mapType = React.useContext(MapContext)!.mapType

    switch (mapType) {
        case 'city':
            return (
                <div className={styles.container}>
                    <CitySVG />
                </div>
            )
        case 'mine':
            return (
                <div className={styles.container}>
                    <MineSVG />
                </div>
            )
        case 'floor':
            return (
                <div className={styles.container}>
                    <FloorSVG />
                </div>
            )
    }
}

export default Map