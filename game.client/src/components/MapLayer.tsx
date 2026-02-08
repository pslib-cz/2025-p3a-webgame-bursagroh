import React from "react"
import { IsBluredContext } from "../providers/global/IsBluredProvider"
import Layer from "./wrappers/layer/Layer"
import Map from "./Map"

const MapLayer = () => {
    const isBlured = React.useContext(IsBluredContext)!.isBlured

    return (
        <Layer layer={0} isBlured={isBlured}>
            <Map pointerEvents={isBlured ? "none" : "auto"} />
        </Layer>
    )
}

export default MapLayer