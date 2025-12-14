import React from "react"
import SVGDisplay from "../SVGDisplay"
import Tile from "../SVG/Tile"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"

const CityScreen = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    return (
        <SVGDisplay width={"99vw"} height={"99vh"}>
            <Tile width={1} height={1} x={0} y={0} tileType="bank" />
        </SVGDisplay>
    )
}

export default CityScreen
