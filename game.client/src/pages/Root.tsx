import { NavLink } from "react-router"
import { PlayerIdContext } from "../providers/PlayerIdProvider"
import React from "react"

const Root = () => {
    const { generatePlayerIdAsync } = React.useContext(PlayerIdContext)!

    const handleClick = () => {
        generatePlayerIdAsync()
    }

    return (
        <NavLink to="/game/city" onClick={handleClick}>
            Start
        </NavLink>
    )
}

export default Root
