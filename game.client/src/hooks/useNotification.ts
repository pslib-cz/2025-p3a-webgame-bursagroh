import React from 'react'
import { NotificationContext } from '../providers/NotificationProvider'
import type { APIError } from '../types/api'

const useNotification = () => {
    const { notify } = React.useContext(NotificationContext)!

    const genericError = (error: Error) => {
        const errorData = JSON.parse(error.message) as APIError

        if (errorData.notification) {
            notify(errorData.notification.heading, errorData.notification.text, 2000)
        }
    }

    return { notify, genericError }
}

export default useNotification