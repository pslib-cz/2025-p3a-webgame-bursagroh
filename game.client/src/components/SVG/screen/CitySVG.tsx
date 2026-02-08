import React from 'react'
import { PlayerContext } from '../../../providers/global/PlayerProvider'
import { getChunkList } from '../../../utils/map'
import SVGDisplay from '../../SVGDisplay'
import Chunk from '../Chunk'
import Asset from '../Asset'
import styles from './citySVG.module.css'
import Tooltip from '../../Tooltip'

const chunkSize = 16
const horizontalViewDistanceInChunks = 1
const verticalViewDistanceInChunks = 1

const CitySVG = () => {
    const player = React.useContext(PlayerContext)?.player

    if (!player) {
        return <div>!!! Dummy MAP HERE !!!</div>
    }

    const chunks = getChunkList(player.positionX, player.positionY, horizontalViewDistanceInChunks, verticalViewDistanceInChunks, chunkSize)

    return (
        <SVGDisplay centerX={player.positionX} centerY={player.positionY} className={styles.city}>
            {chunks.map((chunk) => <Chunk key={`x:${chunk.x};y:${chunk.y}`} x={chunk.x} y={chunk.y} size={chunkSize} />)}
            <Tooltip heading='Player' text={`Player is located at x: ${player.positionX} y: ${player.positionY}`}>
                <Asset assetType='player' x={player.positionX} y={player.positionY} width={1} height={1} />
            </Tooltip>
        </SVGDisplay>
    )
}

export default CitySVG