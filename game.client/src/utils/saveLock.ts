let saving = false
let waiters: Array<() => void> = []

export const beginSave = () => {
    saving = true
}

export const endSave = () => {
    saving = false
    if (waiters.length) {
        const pending = waiters
        waiters = []
        pending.forEach((resolve) => resolve())
    }
}

export const waitForSaveIdle = () => {
    if (!saving) return Promise.resolve()
    return new Promise<void>((resolve) => {
        waiters.push(resolve)
    })
}