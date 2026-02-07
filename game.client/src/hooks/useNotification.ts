import React from 'react'
import { NotificationContext } from '../providers/NotificationProvider'

const useNotification = () => {
    const { notify } = React.useContext(NotificationContext)!

    return notify
}

export default useNotification