import React from 'react'
import styles from './notification.module.css'
import CloseIcon from '../assets/icons/CloseIcon'
import { NotificationContext, type Notification as NotificationType } from '../providers/global/NotificationProvider'

const Notification: React.FC<NotificationType> = ({ heading, text, id }) => {
    const { removeNotification } = React.useContext(NotificationContext)!

    const handleClose = () => {
        removeNotification(id)
    }

    return (
        <div className={styles.container}>
            <span className={styles.heading}>{heading}</span>
            <div className={styles.innerContainer}>
                <span className={styles.text}>{text}</span>
            </div>
            <CloseIcon className={styles.close} width={24} height={24} onClick={handleClose} />
        </div>
    )
}

export default Notification