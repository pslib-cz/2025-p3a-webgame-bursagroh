import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import "normalize.css"
import { createBrowserRouter } from "react-router"
import { RouterProvider } from "react-router/dom"
import Game from "./pages/Game.tsx"
import Root from "./pages/Root.tsx"
import Layout from "./pages/Layout.tsx"

const router = createBrowserRouter([
    {
        path: "/",
        Component: Layout,
        children: [
            {
                index: true,
                Component: Root,
            },
            {
                path: "game",
                Component: Game,
            }
        ]
    }
])

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <RouterProvider router={router} />
    </StrictMode>
)
