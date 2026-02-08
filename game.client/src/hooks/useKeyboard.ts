import React from "react"

type KeyboardHandler = (event: KeyboardEvent) => void

type UseKeyboardOptions = {
    eventTarget?: Window | Document | HTMLElement
    preventDefault?: boolean
    enabled?: boolean
}

const useKeyboard = (key: string, handler: KeyboardHandler, options: UseKeyboardOptions = {}) => {
    const { eventTarget = window, preventDefault = false, enabled = true } = options
    const handlerRef = React.useRef(handler)

    React.useEffect(() => {
        handlerRef.current = handler
    }, [handler])

    React.useEffect(() => {
        if (!enabled) return

        const onKeyDown = (event: Event) => {
            const keyboardEvent = event as KeyboardEvent
            if (keyboardEvent.key !== key) return
            if (preventDefault) keyboardEvent.preventDefault()
            handlerRef.current(keyboardEvent)
        }

        eventTarget.addEventListener("keydown", onKeyDown)
        return () => eventTarget.removeEventListener("keydown", onKeyDown)
    }, [key, eventTarget, preventDefault, enabled])
}

export default useKeyboard
