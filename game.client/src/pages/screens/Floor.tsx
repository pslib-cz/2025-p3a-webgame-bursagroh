import React from 'react'
import { PlayerIdContext } from '../../providers/PlayerIdProvider'
import { useMutation, useQuery } from '@tanstack/react-query'
import { getPlayerQuery, updatePlayerScreenMutation } from '../../api/player'
import { useNavigate } from 'react-router'
import type { TileType } from '../../types'
import { getBuildingFloorQuery, getBuildingsQuery } from '../../api/building'
import type { Player as PlayerType } from '../../types/api/models/player'
import SVGDisplay from '../../components/SVGDisplay'
import FloorSVG from '../../components/SVG/Floor'
import Tile from '../../components/SVG/Tile'
import type { Building, EnemyType } from '../../types/api/models/building'
import { BuildingIdContext } from '../../providers/BuildingIdProvider'
import { LayerContext } from '../../providers/LayerProvider'
import Asset from '../../components/SVG/Asset'
import { FloorContext } from '../../providers/FloorProvider'
import { PlayerContext } from '../../providers/game/PlayerProvider'
import useBlur from '../../hooks/useBlur'
import useMap from '../../hooks/useMap'

// const mapItemIdToTileType = (itemId: number): TileType => {
//     switch (itemId) {
//         case 1:
//             return "wood"
//         case 2:
//             return "rock_item"
//         case 3:
//             return "copper"
//         case 4:
//             return "iron"
//         case 5:
//             return "silver"
//         case 6:
//             return "gold"
//         case 7:
//             return "unobtainium"
//         case 10:
//             return "wooden_sword"
//         case 30:
//             return "wooden_pickaxe"
//         case 39:
//             return "wooden_pickaxe"
//         default:
//             return "empty"
//     }
// }

// const BuildingFetch = ({player, level}: {player: PlayerType, level: number}) => {
//     const buildingIdContext = React.useContext(BuildingIdContext)!
//     const {data, isPending, isError, isSuccess} = useQuery(getBuildingsQuery(player.playerId, player.positionY, player.positionX, 1, 1))

//     if (isPending) {
//         return <div>Loading...</div>
//     }

//     if (isError) {
//         return <div>Error</div>
//     }

//     if (isSuccess) {
//         buildingIdContext.setBuildingId(data[0].buildingId)
//         return <Floor player={player} building={data[0]} level={level} />
//     }
// }

// const Floor = ({player, building, level}: {player: PlayerType, building: Building, level: number}) => {
//     const navigate = useNavigate()
//     const {data, isPending, isError, isSuccess} = useQuery(getBuildingFloorQuery(player.playerId, building.buildingId, level))

//     const floorAbove = useQuery(getBuildingFloorQuery(player.playerId, building.buildingId, (building.buildingType === "Abandoned" || building.buildingType === "AbandonedTrap") && building.height === level ? level : level + 1))
//     const floorBelow = useQuery(getBuildingFloorQuery(player.playerId, building.buildingId, level === 0 ? 0 : level - 1))

//     const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(player.playerId, "City"))

//     const handleClick = async () => {
//         await updatePlayerScreenAsync()

//         navigate("/game/city")
//     }

//     if (isPending) {
//         return <div>Loading...</div>
//     }

//     if (isError) {
//         return <div>Error</div>
//     }

//     if (isSuccess) {
//         return (
//             <div>
//                 <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.subPositionX} centerY={player.subPositionY}>
//                     <FloorSVG />
//                     {data.floorItems.map((item) => {
//                         if (item.floorItemType === "Stair") {
//                             const targetLevel = (level ?? 0) + (item.positionX > 3 ? 1 : -1)
//                             const targetFloorId = item.positionX > 3 ? floorAbove.data?.floorId : floorBelow.data?.floorId

//                             return (
//                                 <Tile z-index={100} key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} tileType='stair' targetLevel={targetLevel} targetFloorId={targetFloorId} />
//                             )
//                         }

//                         if (item.floorItemType === "Enemy" && item.enemy) {
//                             return (
//                                 <Tile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} tileType={mapEnemyTypeToTileType(item.enemy.enemyType)} targetBuildingId={building.buildingId} targetLevel={level} />
//                             )
//                         }

//                         if (item.floorItemType === "Item" && item.itemInstance) {
//                             return (
//                                 <Tile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} tileType={mapItemIdToTileType(item.itemInstance.item.itemId)} targetFloorItemId={item.floorItemId} targetBuildingId={building.buildingId} targetLevel={level} />
//                             )
//                         }
//                     })}
//                     <Asset assetType='player' x={player.subPositionX} y={player.subPositionY} width={1} height={1} />
//                 </SVGDisplay>
//                 Floor - {data.level}
//                 <button onClick={handleClick}>close</button>
//             </div>
//         )
//     }
// }

const FloorScreen = () => {
    useBlur(false)
    useMap("floor")
    // const layerContext = React.useContext(LayerContext)!
    // const playerId = React.useContext(PlayerIdContext)!.playerId!
    // const {data, isPending, isError, isSuccess} = useQuery(getPlayerQuery(playerId))

    // if (isPending) {
    //     return <div>Loading...</div>
    // }

    // if (isError) {
    //     return <div>Error</div>
    // }

    // if (isSuccess) {
    //     layerContext.setLayer(Number(params.level))
    //     return <BuildingFetch player={data} level={Number(params.level)} />
    // }

    return null

    // const floor = React.useContext(FloorContext)!.floor
    // const player = React.useContext(PlayerContext)!.player!

    // if (!floor) {
    //     return <div>Loading floor...</div>
    // }

    // return (
    //     <div>
    //         <SVGDisplay width={"99vw"} height={"99vh"} centerX={player.subPositionX} centerY={player.subPositionY}>
    //             <FloorSVG />
    //             {floor.floorItems.map((item) => {
    //                 if (item.floorItemType === "Stair") {
    //                     return (
    //                         <Tile z-index={100} key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} tileType='stair' targetLevel={0} targetFloorId={0} />
    //                     )
    //                 }

    //                 if (item.floorItemType === "Enemy" && item.enemy) {
    //                     return (
    //                         <Tile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={1} height={1} tileType={mapEnemyTypeToTileType(item.enemy.enemyType)} targetBuildingId={0} targetLevel={0} />
    //                     )
    //                 }

    //                 // if (item.floorItemType === "Item" && item.itemInstance) {
    //                 //     return (
    //                 //         <Tile key={`x:${item.positionX};y:${item.positionY}`} x={item.positionX} y={item.positionY} width={0.5} height={0.5} tileType={mapItemIdToTileType(item.itemInstance.item.itemId)} targetFloorItemId={item.floorItemId} targetBuildingId={building.buildingId} targetLevel={level} />
    //                 //     )
    //                 // }
    //             })}
    //             <Asset assetType='player' x={player.subPositionX} y={player.subPositionY} width={1} height={1} />
    //         </SVGDisplay>
    //         Floor - {floor.level}
    //         {/* <button onClick={handleClick}>close</button> */}
    //     </div>
    // )
}

export default FloorScreen