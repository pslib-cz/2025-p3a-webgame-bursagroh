import { Outlet } from "react-router"
import Providers from "../../providers"
import Layer from "../../components/wrappers/layer/Layer"
import Map from "../../components/Map"
import { IsBluredContext } from "../../providers/global/IsBluredProvider"
import React from "react"
import Layers from "../../components/wrappers/layer/Layers"
import Notifications from "../../components/Notifications"
import TooltipLayer from "../../components/TooltipLayer"

const MapLayer = () => {
    const isBlured = React.useContext(IsBluredContext)!.isBlured

    return (
        <Layer layer={0} isBlured={isBlured}>
            <Map />
        </Layer>
    )
}

const Layout = () => {
    return (
        <Providers>
            <Layers>
                <MapLayer />
                <Outlet />
                <Notifications />
                <TooltipLayer />
            </Layers>
        </Providers>
    )
}

export default Layout
