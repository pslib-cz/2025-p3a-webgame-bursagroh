import SVGDisplay from "./components/SVGDisplay"
import Tile from "./components/SVG/Tile"

function App() {
    return (
        <>
            <SVGDisplay width={"99vw"} height={"99vh"}>
                {/* {Array.from({length: 1}).map((_, rowIndex) =>
                    Array.from({length: 1}).map((_, columnIndex) => (
                        <Tile key={`${rowIndex - 5}-${columnIndex - 5}`} width={1} height={1} x={columnIndex - 5} y={rowIndex - 5} tileType="bank" />
                    ))
                )} */}
                <Tile width={1} height={1} x={0} y={0} tileType="bank" />
            </SVGDisplay>
        </>
    )
}

export default App
