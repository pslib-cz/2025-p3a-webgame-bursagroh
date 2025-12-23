import { Outlet } from "react-router"
import Providers from "../../providers"

const Layout = () => {
    return (
        <Providers>
            <Outlet />
        </Providers>
    )
}

export default Layout
