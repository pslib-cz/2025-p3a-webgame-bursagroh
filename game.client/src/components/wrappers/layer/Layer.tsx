import React from 'react'
import styles from './layer.module.css'

type LayerProps = {
    layer: number
    isBlured?: boolean
}

const Layer: React.FC<React.PropsWithChildren<LayerProps>> = ({ children, layer, isBlured }) => {
    if (isBlured) {
        return (
            <div className={styles.layer} style={{ zIndex: layer }}>
                <div className={styles.layerBlur}>{children}</div>
            </div>
        )
    }

    return (
        <div className={styles.layer} style={{ zIndex: layer }}>{children}</div>
    )
}

export default Layer