import FloorTile from './tiles/floor/FloorTile'

const mapPositionToTileType = (x: number, y: number) => {
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
                return <FloorTile key={`x:${x};y:${y}`} x={x} y={y} width={1} height={1} floorTileType={mapPositionToTileType(x, y)} />
            }))}
        </>
    )
}

export default Floor