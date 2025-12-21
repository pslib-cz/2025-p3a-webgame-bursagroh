import React from "react"

type DisplayProps = {
    displayWidth?: number
    displayHeight?: number
} & React.PropsWithChildren &
    Omit<React.SVGProps<SVGSVGElement>, "preserveAspectRatio" | "viewBox" | "xmlns">

const SVGDisplay: React.FC<DisplayProps> = ({ children, displayWidth = 10, displayHeight = 10, ...props }) => {
    return (
        <svg {...props} viewBox={`${-displayWidth / 2} ${-displayHeight / 2} ${displayWidth} ${displayHeight}`} preserveAspectRatio="xMidYMid meet" xmlns="http://www.w3.org/2000/svg">
            {children}
        </svg>
    )
}

export default SVGDisplay
