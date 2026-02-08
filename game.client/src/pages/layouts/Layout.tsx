import { Outlet } from "react-router"
import Providers from "../../providers"
import Layers from "../../components/wrappers/layer/Layers"
import Notifications from "../../components/Notifications"
import TooltipLayer from "../../components/TooltipLayer"
import MapLayer from "../../components/MapLayer"
import UnsupportedResolutionLayer from "../../components/UnsupportedResolutionLayer"

const Layout = () => {
    return (
        <Providers>
            <Layers>
                <MapLayer />
                <Outlet />
                <Notifications />
                <TooltipLayer />
                <UnsupportedResolutionLayer />
            </Layers>
        </Providers>
    )
}

export default Layout
