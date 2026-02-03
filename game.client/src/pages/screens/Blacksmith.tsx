import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useMutation, useQuery } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import { getBlueprintsQuery, getPlayerBlueprintsQuery } from "../../api/blueprint"
import { useNavigate } from "react-router"
import styles from "./blacksmith.module.css"
import BlueprintItem from "../../components/item/BlueprintItem"
import Crafting from "../../components/Crafting"
import useBlur from "../../hooks/useBlur"

const BlacksmithScreen = () => {
    useBlur(true)
    
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))

    const blueprints = useQuery(getBlueprintsQuery())
    const playerBlueprints = useQuery(getPlayerBlueprintsQuery(playerId))

    const handleClick = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    if (blueprints.isLoading || playerBlueprints.isLoading) {
        return <div>Loading...</div>
    }

    if (blueprints.isError || playerBlueprints.isError) {
        return <div>Error loading blueprints.</div>
    }

    if (blueprints.isSuccess && playerBlueprints.isSuccess) {
        const blueprintsToBuy = blueprints.data.filter((blueprint) => !playerBlueprints.data.some((playerBlueprint) => playerBlueprint.blueprintId === blueprint.blueprintId))

        return (
            <div className={styles.container}>
                <div className={styles.blacksmithContainer}>
                    <span className={styles.heading}>Crafting</span>
                    <span className={styles.heading}>Blueprint</span>
                    <div className={styles.craftingContainer} >
                        {playerBlueprints.data.map((blueprint) => (
                            <Crafting blueprint={blueprint} key={blueprint.blueprintId} />
                        ))}
                    </div>
                    <div className={styles.blueprintContainer}>
                        {blueprintsToBuy.map((blueprint) => (
                            <BlueprintItem blueprint={blueprint} key={blueprint.blueprintId} />
                        ))}
                    </div>
                    <button className={styles.close} onClick={handleClick}>close</button>
                </div>
            </div>
        )
    }
}

export default BlacksmithScreen
