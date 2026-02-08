import React from "react"
import type { Notification } from "../../types/notification"

type NotificationContextType = {
    notify: (heading: string, text: string, timer?: number) => void
    removeNotification: (id: number) => void
    notifications: Notification[]
}

// eslint-disable-next-line react-refresh/only-export-components
export const NotificationContext = React.createContext<NotificationContextType | null>(null)

const NotificationProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [idCounter, setIdCounter] = React.useState(0)
    const [notifications, setNotifications] = React.useState<Notification[]>([])

    const removeNotification = (id: number) => {
        setNotifications((prev) => prev.filter((notification) => notification.id !== id))
    }

    const notify = (heading: string, text: string, timer?: number) => {
        setNotifications((prev) => [...prev, { id: idCounter, heading, text }])
        setIdCounter((prev) => prev + 1)
        if (timer) {
            setTimeout(() => {
                removeNotification(idCounter)
            }, timer)
        }
    }

    return <NotificationContext.Provider value={{ notify, removeNotification, notifications }}>{children}</NotificationContext.Provider>
}

export default NotificationProvider
