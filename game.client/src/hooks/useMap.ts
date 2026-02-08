import React from 'react'
import type { MapType } from '../types/map'
import { MapContext } from '../providers/global/MapProvider'

const useMap = (mapType: MapType) => {
    const setMapType = React.useContext(MapContext)!.setMapType
    
    React.useEffect(() => {
        setMapType(mapType)
    }, [mapType, setMapType])
}

export default useMap