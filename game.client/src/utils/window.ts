export const getWindowSize = () => ({ width: window.innerWidth, height: window.innerHeight })

export const readCSSProperty = (name: string) => {
    const raw = getComputedStyle(document.documentElement)
        .getPropertyValue(name)
        .trim()

    return Number.parseFloat(raw)
}