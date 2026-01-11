import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useMutation, useQuery } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import { buyBlueprintMutation, craftBlueprintMutation, getBlueprintsQuery, getPlayerBlueprintsQuery } from "../../api/blueprint"
import type { Blueprint } from "../../types/api/models/blueprint"
import { useNavigate } from "react-router"

const BlueprintToBuy = ({ blueprint }: {blueprint: Blueprint}) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const {mutateAsync: buyBlueprintAsync} = useMutation(buyBlueprintMutation(playerId, blueprint.blueprintId))

    const handleClick = () => {
        buyBlueprintAsync()
    }

    return (
        <>
            <h3>{blueprint.item.name}</h3>
            <p>{blueprint.price}</p>
            <button onClick={handleClick}>Buy</button>
        </>
    )
}

const BlueprintToCraft = ({ blueprint }: {blueprint: Blueprint}) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const {mutateAsync: craftBlueprintAsync} = useMutation(craftBlueprintMutation(playerId, blueprint.blueprintId))

    const handleClick = () => {
        craftBlueprintAsync()
    }

    return (
        <>
            <h3>{blueprint.item.name}</h3>
            <p>{blueprint.price}</p>
            {blueprint.craftings.map((crafting) => (
                <div key={crafting.craftingId}>
                    <p>{crafting.itemId}: {crafting.amount}x</p>
                </div>
            ))}
            <button onClick={handleClick}>Craft</button>
        </>
    )
}

const BlacksmithScreen = () => {
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
            <>
                <p>Blacksmith</p>
                <button onClick={handleClick}>close</button>
                {blueprintsToBuy.map((blueprint) => (
                    <BlueprintToBuy blueprint={blueprint} key={blueprint.blueprintId} />
                ))}
                {playerBlueprints.data.map((blueprint) => (
                    <BlueprintToCraft blueprint={blueprint} key={blueprint.blueprintId} />
                ))}
            </>
        )
    }
}

export default BlacksmithScreen
