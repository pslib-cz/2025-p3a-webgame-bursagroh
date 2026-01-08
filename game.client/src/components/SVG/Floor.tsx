import Tile from './Tile'
import type { TileType } from '../../types'

const mapPositionToTileType = (x: number, y: number): TileType => {
    if (x === 0 && y === 0) return "wall-top-left"
    if (x === 0 && y === 7) return "wall-bottom-left"
    if (x === 7 && y === 0) return "wall-top-right"
    if (x === 7 && y === 7) return "wall-bottom-right"
    if (y === 0) return "wall-top"
    if (y === 7) return "wall-bottom"
    if (x === 0) return "wall-left"
    if (x === 7) return "wall-right"
    return "floor"
}

const Floor = () => {
  return (
    <>
        {new Array(8).fill(null).flatMap((_, y) => new Array(8).fill(null).map((_, x) => {
            return <Tile key={`x:${x};y:${y}`} x={x} y={y} width={1} height={1} tileType={mapPositionToTileType(x, y)} />
        }))}
    </>
  )
}

export default Floor