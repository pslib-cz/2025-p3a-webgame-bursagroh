import React from "react"
import { MineItemsContext } from "../../providers/game/MineItemsProvider"
import { itemIdToAssetType } from "../../utils/item"
import Asset from "./Asset"

const MineItems = () => {
    const mineItems = React.useContext(MineItemsContext)!.mineItems!

    return (
        <>
            {mineItems.map((item) => (
                <Asset key={`mineItem:${item.floorItemId}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} assetType={itemIdToAssetType(item.itemInstance.item.itemId)} pointerEvents="none" />
            ))}
        </>
    )
}

export default MineItems