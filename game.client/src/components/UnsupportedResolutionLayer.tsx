import React from "react"
import Layer from "./wrappers/layer/Layer"
import styles from "./unsupportedResolutionLayer.module.css"
import { MIN_SUPPORTED_HEIGHT, MIN_SUPPORTED_WIDTH } from "../constants/window"
import { getWindowSize } from "../utils/window"

const UnsupportedResolutionLayer = () => {
    const [size, setSize] = React.useState(getWindowSize)

    React.useEffect(() => {
        const onResize = () => setSize(getWindowSize())
        window.addEventListener("resize", onResize)
        return () => window.removeEventListener("resize", onResize)
    }, [])

    const isSupported = size.width >= MIN_SUPPORTED_WIDTH && size.height >= MIN_SUPPORTED_HEIGHT

    if (isSupported) return null

    return (
        <Layer layer={5}>
            <div className={styles.container}>
                <span className={styles.text}>Increase your window size to at least {MIN_SUPPORTED_WIDTH}x{MIN_SUPPORTED_HEIGHT}</span>
            </div>
        </Layer>
    )
}

export default UnsupportedResolutionLayer