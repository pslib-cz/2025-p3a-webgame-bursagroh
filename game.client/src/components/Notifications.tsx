import React from 'react'
import styles from './notifications.module.css'
import Layer from './wrappers/layer/Layer'
import Notification from './Notification'
import { NotificationContext } from '../providers/NotificationProvider'

const Notifications = () => {
    const { notifications } = React.useContext(NotificationContext)!

    return (
        <Layer layer={3}>
            <div className={styles.container}>
                {notifications.map((notification) => (
                    <Notification key={notification.id} {...notification} />
                ))}
            </div>
        </Layer>
    )
}

export default Notifications