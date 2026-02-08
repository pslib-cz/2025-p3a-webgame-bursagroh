import React from "react"
import { PlayerIdContext } from "../../providers/global/PlayerIdProvider"
import { useMutation, useQuery } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import { getBlueprintsQuery, getPlayerBlueprintsQuery } from "../../api/blueprint"
import { useNavigate } from "react-router"
import styles from "./blacksmith.module.css"
import BlueprintItem from "../../components/item/BlueprintItem"
import Crafting from "../../components/Crafting"
import useBlur from "../../hooks/useBlur"
import CloseIcon from "../../assets/icons/CloseIcon"
import useNotification from "../../hooks/useNotification"
import useKeyboard from "../../hooks/useKeyboard"
import ArrayDisplay from "../../components/wrappers/ArrayDisplay"

const BlacksmithScreen = () => {
    useBlur(true)
    
    const navigate = useNavigate()
    const {genericError} = useNotification()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City", genericError))

    const blueprints = useQuery(getBlueprintsQuery())
    const playerBlueprints = useQuery(getPlayerBlueprintsQuery(playerId))

    const handleEscape = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    useKeyboard("Escape", handleEscape)

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
                        <ArrayDisplay elements={playerBlueprints.data.map((blueprint) => (
                            <Crafting blueprint={blueprint} key={blueprint.blueprintId} />
                        ))} ifEmpty={<span className={styles.text}>No blueprints available</span>} />
                    </div>
                    <div className={styles.blueprintContainer}>
                        <ArrayDisplay elements={blueprintsToBuy.map((blueprint) => (
                            <BlueprintItem blueprint={blueprint} key={blueprint.blueprintId} />
                        ))} ifEmpty={<span className={styles.text}>No blueprints available</span>} />
                    </div>
                    <CloseIcon className={styles.close} onClick={handleEscape} width={24} height={24} />
                </div>
            </div>
        )
    }
}

export default BlacksmithScreen
