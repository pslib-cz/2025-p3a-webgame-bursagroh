import React from 'react'
import { TooltipContext } from '../providers/TooltipProvider'

type TooltipProps = {
    heading: string
    text: string
    relativeX?: number
    relativeY?: number
}

const Tooltip: React.FC<React.PropsWithChildren<TooltipProps>> = ({ children, heading, text, relativeX = 12, relativeY = 16 }) => {
    const id = React.useId()

    const [open, setOpen] = React.useState(false)
    const [pos, setPos] = React.useState({ x: 0, y: 0 })

    const { setTooltip } = React.useContext(TooltipContext)!

    const onMouseEnter = (e: React.MouseEvent) => {
        setOpen(true)
        setPos({ x: e.clientX, y: e.clientY })
    }

    const onMouseLeave = () => {
        setOpen(false)
    }

    const onMove = (e: React.MouseEvent) => {
        setPos({ x: e.clientX, y: e.clientY })
    }

    React.useEffect(() => {
        if (open) {
            setTooltip({ mouseX: pos.x, mouseY: pos.y, relativeX, relativeY, heading, text, id })
        } else {
            setTooltip(null)
        }

        return () => {
            setTooltip((prev) => (prev?.id === id ? null : prev))
        }
    }, [open, pos, setTooltip, heading, text, id, relativeX, relativeY])

    return (
        <div onMouseEnter={onMouseEnter} onMouseLeave={onMouseLeave} onMouseMove={onMove}>{children}</div>
    )
}

export default Tooltip