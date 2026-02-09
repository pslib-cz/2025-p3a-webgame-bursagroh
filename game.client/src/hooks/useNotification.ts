import React from 'react'
import { NotificationContext } from '../providers/global/NotificationProvider'
import type { APIError } from '../types/api'
import { GENERIC_ERROR_NOTIFICATION_TIME } from '../constants/notification'

const useNotification = () => {
    const { notify } = React.useContext(NotificationContext)!

    const genericError = (error: Error) => {
        const errorData = JSON.parse(error.message) as APIError

        if (errorData.notification) {
            notify(errorData.notification.heading, errorData.notification.text, GENERIC_ERROR_NOTIFICATION_TIME)
        }
    }

    return { notify, genericError }
}

export default useNotification