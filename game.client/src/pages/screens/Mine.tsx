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
import useBlur from "../../hooks/useBlur"
import useMap from "../../hooks/useMap"





const MineScreen = () => {
    useBlur(false)
    useMap("mine")
    
    // const navigate = useNavigate()
    // const playerId = React.useContext(PlayerIdContext)!.playerId!
    // const mineIdContext = React.useContext(MineIdContext)!
    // const player = useQuery(getPlayerQuery(playerId))
    // const mine = useQuery(generateMineQuery(playerId))

    // const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))
    // const { mutateAsync: rentPickAsync } = useMutation(rentPickMutation(playerId, 1))

    // if (player.isError || mine.isError) {
    //     return <div>Error loading.</div>
    // }

    // if (player.isPending || mine.data === null) {
    //     return <div>Loading mine...</div>
    // }

    // const handleLeave = async () => {
    //     await updatePlayerScreenAsync()

    //     navigate("/game/city")
    // }

    // const handleBuy = async () => {
    //     await rentPickAsync()
    // }

    // if (player.isSuccess && mine.isSuccess) {
    //     mineIdContext.setMineId(mine.data.mine.mineId)
    //     const layers = getLayerList(player.data.subPositionY, viewDistanceInChunks, chunkSize)

    //     return (
    //         <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.data.subPositionX} centerY={player.data.subPositionY}>
    //             <Asset assetType='table_left' x={1} y={-3} width={1} height={1} onClick={handleLeave} />
    //             <Asset assetType='table_right' x={2} y={-3} width={1} height={1} onClick={handleBuy} />

    //             <Tile x={0} y={-1} width={1} height={1} tileType="empty" />
    //             <Tile x={1} y={-1} width={1} height={1} tileType="empty" />
    //             <Tile x={2} y={-1} width={1} height={1} tileType="empty" />
    //             <Tile x={3} y={-1} width={1} height={1} tileType="empty" />
    //             <Tile x={4} y={-1} width={1} height={1} tileType="empty" />
    //             <Tile x={5} y={-1} width={1} height={1} tileType="empty" />
    //             <Tile x={6} y={-1} width={1} height={1} tileType="empty" />
    //             <Tile x={7} y={-1} width={1} height={1} tileType="empty" />

    //             <Tile x={1} y={-2} width={1} height={1} tileType="empty" />
    //             <Tile x={2} y={-2} width={1} height={1} tileType="empty" />
    //             <Tile x={3} y={-2} width={1} height={1} tileType="empty" />
    //             <Tile x={4} y={-2} width={1} height={1} tileType="empty" />
    //             <Tile x={5} y={-2} width={1} height={1} tileType="empty" />
    //             <Tile x={6} y={-2} width={1} height={1} tileType="empty" />
    //             <Tile x={7} y={-2} width={1} height={1} tileType="empty" />

    //             {layers.map((depth) => (
    //                 <Layer key={`depth:${depth}`} mineId={mine.data.mine.mineId} depth={depth} size={chunkSize} />
    //             ))}

    //             <DisplayMineItems mineId={mine.data.mine.mineId} />
                
    //             <Asset assetType='player' x={player.data.subPositionX} y={player.data.subPositionY} width={1} height={1} />
    //         </SVGDisplay>
    //     )
    // }

    return null
}

export default MineScreen
