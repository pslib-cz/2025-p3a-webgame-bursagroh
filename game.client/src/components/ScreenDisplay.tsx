import { useQuery } from "@tanstack/react-query"
import React from "react"
import { getPlayerQuery } from "../api/player"
import ConditionalDisplay from "./ConditionalDisplay"
import BankScreen from "./screens/Bank"
import BlacksmithScreen from "./screens/Blacksmith"
import FightScreen from "./screens/Fight"
import RestaurantScreen from "./screens/Restaurant"
import MineScreen from "./screens/Mine"
import CityScreen from "./screens/City"
import { PlayerIdContext } from "../providers/PlayerIdProvider"
import styles from "./screenDisplay.module.css"

const ScreenDisplay = () => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { data, isError, isPending, isSuccess } = useQuery(getPlayerQuery(playerId))

    if (isError) {
        return <div>Error loading player data.</div>
    }

    if (isPending) {
        return <div>Loading player data...</div>
    }

    if (isSuccess) {
        return (
            <div className={styles["screen-display"]}>
                <ConditionalDisplay visible={data.screenType === "City"}>
                    <CityScreen />
                </ConditionalDisplay>
                <ConditionalDisplay visible={data.screenType === "Bank"}>
                    <BankScreen />
                </ConditionalDisplay>
                <ConditionalDisplay visible={data.screenType === "Blacksmith"}>
                    <BlacksmithScreen />
                </ConditionalDisplay>
                <ConditionalDisplay visible={data.screenType === "Fight"}>
                    <FightScreen />
                </ConditionalDisplay>
                <ConditionalDisplay visible={data.screenType === "Restaurant"}>
                    <RestaurantScreen />
                </ConditionalDisplay>
                <ConditionalDisplay visible={data.screenType === "Mine"}>
                    <MineScreen />
                </ConditionalDisplay>
            </div>
        )
    }
}

export default ScreenDisplay
