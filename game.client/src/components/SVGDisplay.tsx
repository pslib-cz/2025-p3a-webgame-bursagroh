import React from "react"

type DisplayProps = {
    displayWidth?: number
    displayHeight?: number
    centerX?: number
    centerY?: number
} & React.PropsWithChildren &
    Omit<React.SVGProps<SVGSVGElement>, "preserveAspectRatio" | "viewBox" | "xmlns">

const SVGDisplay: React.FC<DisplayProps> = ({ children, displayWidth = 10, displayHeight = 10, centerX = 0, centerY = 0, ...props }) => {
    return (
        <svg {...props} viewBox={`${centerX - displayWidth / 2} ${centerY - displayHeight / 2} ${displayWidth} ${displayHeight}`} preserveAspectRatio="xMidYMid meet" xmlns="http://www.w3.org/2000/svg">
            {/* <AssetImporter /> */}
            {children}
        </svg>
    )
}

export default SVGDisplay
