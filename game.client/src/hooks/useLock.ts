import React from 'react'

const useLock = () => {
    const lock = React.useRef(false)

    const handleLock = async <T,>(asyncFunction: () => Promise<T>): Promise<T | undefined> => {
        if (lock.current) return

        lock.current = true
        try {
            return await asyncFunction()
        } finally {
            lock.current = false
        }
    }

    return handleLock
}

export default useLock