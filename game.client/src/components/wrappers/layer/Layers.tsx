import React from 'react'
import styles from './layers.module.css'

const Layers: React.FC<React.PropsWithChildren> = ({children}) => {
    return (
        <div className={styles.layers}>{children}</div>
    )
}

export default Layers