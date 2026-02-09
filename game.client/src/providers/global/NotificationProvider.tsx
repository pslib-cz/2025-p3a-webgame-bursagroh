import React from "react"
import type { Notification } from "../../types/notification"
import { NOTIFICATION_MAX_COUNT } from "../../constants/notification"

type NotificationContextType = {
    notify: (heading: string, text: string, timer?: number) => void
    removeNotification: (id: number) => void
    notifications: Notification[]
}

// eslint-disable-next-line react-refresh/only-export-components
export const NotificationContext = React.createContext<NotificationContextType | null>(null)

const NotificationProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const idCounterRef = React.useRef(0)
    const [notifications, setNotifications] = React.useState<Notification[]>([])

    const removeNotification = (id: number) => {
        setNotifications((prev) => prev.filter((notification) => notification.id !== id))
    }

    const notify = (heading: string, text: string, timer?: number) => {
        const id = idCounterRef.current++

        setNotifications((prev) => [...prev, { id, heading, text }].slice(-NOTIFICATION_MAX_COUNT))

        if (timer !== undefined) {
            setTimeout(() => {
                removeNotification(id)
            }, timer)
        }
    }

    return <NotificationContext.Provider value={{ notify, removeNotification, notifications }}>{children}</NotificationContext.Provider>
}

export default NotificationProvider
