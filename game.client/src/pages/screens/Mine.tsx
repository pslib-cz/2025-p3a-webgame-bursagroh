import React from "react"
import SVGDisplay from "../../components/SVGDisplay"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { useMutation, useQuery } from "@tanstack/react-query"
import { getPlayerQuery, updatePlayerScreenMutation } from "../../api/player"
import Layer from "../../components/SVG/Layer"
import { generateMineQuery, getMineItemsQuery, rentPickMutation } from "../../api/mine"
import { useNavigate } from "react-router"
import Tile from "../../components/SVG/Tile"
import type { TileType } from "../../types"
import { MineIdContext } from "../../providers/MineIdProvider"
import Asset from "../../components/SVG/Asset"

const chunkSize = 8
const viewDistanceInChunks = 2

const getLayerList = (playerPositionY: number, viewDistanceInChunks: number, chunkSize: number): Array<number> => {
    const playerChunkY = playerPositionY - (playerPositionY % chunkSize)
    const viewDistance = viewDistanceInChunks * chunkSize

    const yFrom = playerChunkY - viewDistance

    const height = viewDistanceInChunks * 2

    return new Array(height)
        .fill(0)
        .map((_, yIndex) => yFrom + yIndex * chunkSize)
        .filter((value) => value >= 0)
}

const mapItemIdToTileType = (itemId: number): TileType => {
    switch (itemId) {
        case 1:
            return "wood"
        case 2:
            return "rock_item"
        case 3:
            return "copper"
        case 4:
            return "iron"
        case 5:
            return "silver"
        case 6:
            return "gold"
        case 7:
            return "unobtainium"
        case 10:
            return "wooden_sword"
        case 30:
            return "wooden_pickaxe"
        case 39:
            return "wooden_pickaxe"
        default:
            return "empty"
    }
}

const DisplayMineItems = ({ mineId }: { mineId: number }) => {
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const mineItems = useQuery(getMineItemsQuery(playerId, mineId))

    if (mineItems.isError) {
        return <div>Error loading mine items.</div>
    }

    if (mineItems.isPending) {
        return <div>Loading mine items...</div>
    }

    if (mineItems.isSuccess) {
        return (
            <>
                {mineItems.data.map((item) => (
                    <Tile key={`mineItem:${item.floorItemId}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} tileType={mapItemIdToTileType(item.itemInstance.item.itemId)} targetFloorItemId={item.floorItemId} mineId={mineId} />
                ))}
            </>
        )
    }
}

const MineScreen = () => {
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const mineIdContext = React.useContext(MineIdContext)!
    const player = useQuery(getPlayerQuery(playerId))
    const mine = useQuery(generateMineQuery(playerId))

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))
    const { mutateAsync: rentPickAsync } = useMutation(rentPickMutation(playerId, 1))

    if (player.isError || mine.isError) {
        return <div>Error loading.</div>
    }

    if (player.isPending || mine.data === null) {
        return <div>Loading mine...</div>
    }

    const handleLeave = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    const handleBuy = async () => {
        await rentPickAsync()
    }

    if (player.isSuccess && mine.isSuccess) {
        mineIdContext.setMineId(mine.data.mine.mineId)
        const layers = getLayerList(player.data.subPositionY, viewDistanceInChunks, chunkSize)

        return (
            <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.data.subPositionX} centerY={player.data.subPositionY}>
                <Asset assetType='table_left' x={1} y={-3} width={1} height={1} onClick={handleLeave} />
                <Asset assetType='table_right' x={2} y={-3} width={1} height={1} onClick={handleBuy} />

                <Tile x={0} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={1} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={2} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={3} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={4} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={5} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={6} y={-1} width={1} height={1} tileType="empty" />
                <Tile x={7} y={-1} width={1} height={1} tileType="empty" />

                <Tile x={1} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={2} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={3} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={4} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={5} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={6} y={-2} width={1} height={1} tileType="empty" />
                <Tile x={7} y={-2} width={1} height={1} tileType="empty" />

                {layers.map((depth) => (
                    <Layer key={`depth:${depth}`} mineId={mine.data.mine.mineId} depth={depth} size={chunkSize} />
                ))}

                <DisplayMineItems mineId={mine.data.mine.mineId} />
                
                <Asset assetType='player' x={player.data.subPositionX} y={player.data.subPositionY} width={1} height={1} />
            </SVGDisplay>
        )
    }
}

export default MineScreen
