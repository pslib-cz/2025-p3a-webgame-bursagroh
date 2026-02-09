import React from 'react'
import styles from './itemContainer.module.css'

type ItemContainerProps = {
    itemCount: number
}

const ItemContainer: React.FC<React.PropsWithChildren<ItemContainerProps>> = ({children, itemCount}) => {
    return (
        <div className={styles.itemContainer} style={{ '--item-count': itemCount } as React.CSSProperties}>
            {children}
        </div>
    )
}

export default ItemContainer