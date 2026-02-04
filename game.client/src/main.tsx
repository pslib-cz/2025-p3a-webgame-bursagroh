import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import "normalize.css"
import "./variables.css"
import { createBrowserRouter } from "react-router"
import { RouterProvider } from "react-router/dom"
import Game from "./pages/layouts/Game.tsx"
import Root from "./pages/Root.tsx"
import Layout from "./pages/layouts/Layout.tsx"
import CityScreen from "./pages/screens/City.tsx"
import BankScreen from "./pages/screens/Bank.tsx"
import BlacksmithScreen from "./pages/screens/Blacksmith.tsx"
import FightScreen from "./pages/screens/Fight.tsx"
import MineScreen from "./pages/screens/Mine.tsx"
import RestaurantScreen from "./pages/screens/Restaurant.tsx"
import FloorScreen from "./pages/screens/Floor.tsx"
import FountainScreen from "./pages/screens/Fountain.tsx"
import SettingsScreen from "./pages/screens/Settings.tsx"
import SaveScreen from "./pages/screens/Save.tsx"
import LoadScreen from "./pages/screens/Load.tsx"
import LoadSaveScreen from "./pages/screens/LoadSave.tsx"
import LoseScreen from "./pages/screens/Lose.tsx"
import WinScreen from "./pages/screens/Win.tsx"

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
                children: [
                    {
                        index: false
                    },
                    {
                        path: "city",
                        Component: CityScreen
                        
                    },
                    {
                        path: "bank",
                        Component: BankScreen
                    },
                    {
                        path: "blacksmith",
                        Component: BlacksmithScreen
                    },
                    {
                        path: "fight",
                        Component: FightScreen
                    },
                    {
                        path: "mine",
                        Component: MineScreen
                    },
                    {
                        path: "restaurant",
                        Component: RestaurantScreen
                    },
                    {
                        path: "floor",
                        Component: FloorScreen
                    },
                    {
                        path: "fountain",
                        Component: FountainScreen
                    },
                    {
                        path: "lose",
                        Component: LoseScreen
                    },
                    {
                        path: "win",
                        Component: WinScreen
                    }
                ]
            },
            {
                path: "settings",
                Component: SettingsScreen
            },
            {
                path: "save",
                Component: SaveScreen
            },
            {
                path: "load",
                Component: LoadScreen
            },
            {
                path: "load/:saveString",
                Component: LoadSaveScreen
            }
        ]
    }
])

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <RouterProvider router={router} />
    </StrictMode>
)
