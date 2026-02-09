import React from 'react'
import styles from './notification.module.css'
import CloseIcon from '../icons/CloseIcon'
import { NotificationContext } from '../providers/global/NotificationProvider'
import { type Notification as NotificationType } from "../types/notification"
import Text from './Text'

const Notification: React.FC<NotificationType> = ({ heading, text, id }) => {
    const { removeNotification } = React.useContext(NotificationContext)!

    const handleClose = () => {
        removeNotification(id)
    }

    return (
        <div className={styles.container}>
            <Text size="h4">{heading}</Text>
            <div className={styles.innerContainer}>
                <Text size="h5">{text}</Text>
            </div>
            <CloseIcon className={styles.close} width={24} height={24} onClick={handleClose} />
        </div>
    )
}

export default Notification