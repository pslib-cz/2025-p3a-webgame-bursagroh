import React from 'react'
import Layer from './wrappers/layer/Layer'
import { TooltipContext } from '../providers/global/TooltipProvider'
import styles from './tooltipLayer.module.css'
import Text from './Text'

const TooltipLayer = () => {
    const { tooltip } = React.useContext(TooltipContext)!

    const tooltipRef = React.useRef<HTMLDivElement>(null)
    
    const [pos, setPos] = React.useState({ x: 0, y: 0 })
    const [resizeTick, setResizeTick] = React.useState(0)

    const baseX = tooltip ? tooltip.mouseX + tooltip.relativeX : 0
    const baseY = tooltip ? tooltip.mouseY + tooltip.relativeY : 0

    React.useLayoutEffect(() => {
        if (!tooltip) return
        const node = tooltipRef.current
        if (!node) return

        const rect = node.getBoundingClientRect()
        const margin = 8
        const maxX = Math.max(margin, window.innerWidth - rect.width - margin)
        const maxY = Math.max(margin, window.innerHeight - rect.height - margin)

        const nextX = Math.min(Math.max(baseX, margin), maxX)
        const nextY = Math.min(Math.max(baseY, margin), maxY)

        setPos((prev) => (prev.x === nextX && prev.y === nextY ? prev : { x: nextX, y: nextY }))
    }, [tooltip, baseX, baseY, resizeTick])

    React.useEffect(() => {
        const onResize = () => setResizeTick((tick) => tick + 1)
        window.addEventListener('resize', onResize)
        return () => window.removeEventListener('resize', onResize)
    }, [])

    if (!tooltip) return null

    return (
        <Layer layer={4}>
            <div ref={tooltipRef} className={styles.container} style={{ left: pos.x, top: pos.y }}>
                <Text size="h4">{tooltip.heading}</Text>
                <div className={styles.innerContainer}>
                    <Text size="h5">{tooltip.text}</Text>
                </div>
            </div>
        </Layer>
    )
}

export default TooltipLayer