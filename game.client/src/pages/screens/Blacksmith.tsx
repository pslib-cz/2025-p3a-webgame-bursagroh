import React from "react"
import styles from "./blacksmith.module.css"
import BlueprintItem from "../../components/item/BlueprintItem"
import Crafting from "../../components/Crafting"
import useBlur from "../../hooks/useBlur"
import CloseIcon from "../../icons/CloseIcon"
import useKeyboard from "../../hooks/useKeyboard"
import ArrayDisplay from "../../components/wrappers/ArrayDisplay"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import BlueprintProvider, { BlueprintContext } from "../../providers/game/BlueprintProvider"
import PlayerBlueprintsProvider, { PlayerBlueprintsContext } from "../../providers/game/PlayerBlueprintsProvider"
import type { TLoadingWrapperContextState } from '../../types/context'
import useLink from "../../hooks/useLink"
import Text from "../../components/Text"
import ConditionalDisplay from "../../components/wrappers/ConditionalDisplay"
import ItemContainer from "../../components/item/ItemContainer"

const BlacksmithScreenWithContext = () => {
    useBlur(true)

    const moveToPage = useLink()

    const blueprints = React.useContext(BlueprintContext)!.blueprints!
    const playerBlueprints = React.useContext(PlayerBlueprintsContext)!.blueprints!

    const handleEscape = async () => {
        await moveToPage("city", true)
    }

    useKeyboard("Escape", handleEscape)

    const blueprintsToBuy = blueprints.filter((blueprint) => !playerBlueprints.some((playerBlueprint) => playerBlueprint.blueprintId === blueprint.blueprintId))

    return (
        <div className={styles.container}>
            <div className={styles.blacksmithContainer}>
                <Text size="h3" className={styles.heading}>Crafting</Text>
                <Text size="h3" className={styles.heading}>Blueprint</Text>
                <div className={styles.craftingContainer} style={{ gridTemplateColumns: `repeat(${Math.max(Math.min(Object.keys(playerBlueprints).length, 5), 1)}, max-content)` }}>
                    <ArrayDisplay elements={playerBlueprints.map((blueprint) => (
                        <Crafting blueprint={blueprint} key={blueprint.blueprintId} />
                    ))} ifEmpty={<Text size="h4">No craftings available</Text>} />
                </div>
                <div className={styles.blueprintContainer}>
                    <ConditionalDisplay condition={blueprintsToBuy.length > 0} notMet={<Text size="h4">No blueprints available</Text>}>
                        <ItemContainer itemCount={blueprintsToBuy.length}>
                            {blueprintsToBuy.map((blueprint) => (
                                <BlueprintItem blueprint={blueprint} key={blueprint.blueprintId} />
                            ))}
                        </ItemContainer>
                    </ConditionalDisplay>
                </div>
                <CloseIcon className={styles.close} onClick={handleEscape} />
            </div>
        </div>
    )
}

const BlacksmithScreen = () => {
    return (
        <ProviderGroupLoadingWrapper providers={[BlueprintProvider, PlayerBlueprintsProvider]} contextsToLoad={[BlueprintContext, PlayerBlueprintsContext] as Array<React.Context<TLoadingWrapperContextState>>}>
            <BlacksmithScreenWithContext />
        </ProviderGroupLoadingWrapper>
    )
}

export default BlacksmithScreen
