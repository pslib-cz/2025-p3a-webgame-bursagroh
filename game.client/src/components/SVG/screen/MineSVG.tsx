import React from 'react'
import SVGDisplay from '../../SVGDisplay'
import Asset from '../Asset'
import Layer from '../Layer'
import { PlayerIdContext } from '../../../providers/PlayerIdProvider'
import { getMineItemsQuery, rentPickMutation } from '../../../api/mine'
import { useMutation, useQuery } from '@tanstack/react-query'
import { PlayerContext } from '../../../providers/game/PlayerProvider'
import { updatePlayerScreenMutation } from '../../../api/player'
import { useNavigate } from 'react-router'
import styles from './mineSVG.module.css'
import MineTile from '../tiles/mine/MineTile'
import { itemIdToAssetType } from '../../../utils/item'

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
                    <Asset key={`mineItem:${item.floorItemId}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} assetType={itemIdToAssetType(item.itemInstance.item.itemId)} />
                ))}
            </>
        )
    }
}

const MineSVG = () => {
    const navigate = useNavigate()

    const player = React.useContext(PlayerContext)!.player!

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(player.playerId, "City"))
    const { mutateAsync: rentPickAsync } = useMutation(rentPickMutation(player.playerId, 1))

    const handleLeave = async () => {
        await updatePlayerScreenAsync()
        navigate("/game/city")
    }

    const handleBuy = async () => {
        await rentPickAsync()
    }

    const layers = getLayerList(player.subPositionY, viewDistanceInChunks, chunkSize)

    return (
        <SVGDisplay className={styles.mine} centerX={player.subPositionX} centerY={player.subPositionY}>
            <Asset assetType='table_left' x={1} y={-3} width={1} height={1} onClick={handleLeave} />
            <Asset assetType='table_right' x={2} y={-3} width={1} height={1} onClick={handleBuy} />

            <MineTile x={0} y={-1} width={1} height={1} mineTileType="empty" />
            <MineTile x={1} y={-1} width={1} height={1} mineTileType="empty" />
            <MineTile x={2} y={-1} width={1} height={1} mineTileType="empty" />
            <MineTile x={3} y={-1} width={1} height={1} mineTileType="empty" />
            <MineTile x={4} y={-1} width={1} height={1} mineTileType="empty" />
            <MineTile x={5} y={-1} width={1} height={1} mineTileType="empty" />
            <MineTile x={6} y={-1} width={1} height={1} mineTileType="empty" />
            <MineTile x={7} y={-1} width={1} height={1} mineTileType="empty" />

            <MineTile x={1} y={-2} width={1} height={1} mineTileType="empty" />
            <MineTile x={2} y={-2} width={1} height={1} mineTileType="empty" />
            <MineTile x={3} y={-2} width={1} height={1} mineTileType="empty" />
            <MineTile x={4} y={-2} width={1} height={1} mineTileType="empty" />
            <MineTile x={5} y={-2} width={1} height={1} mineTileType="empty" />
            <MineTile x={6} y={-2} width={1} height={1} mineTileType="empty" />
            <MineTile x={7} y={-2} width={1} height={1} mineTileType="empty" />

            {layers.map((depth) => (
                <Layer key={`depth:${depth}`} mineId={player.mineId} depth={depth} size={chunkSize} />
            ))}

            <DisplayMineItems mineId={player.mineId} />

            <Asset assetType='player' x={player.subPositionX} y={player.subPositionY} width={1} height={1} />
        </SVGDisplay>
    )
}

export default MineSVG