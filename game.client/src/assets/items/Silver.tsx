import React from "react"
import type { AssetProps } from "../../types"

type SilverProps = AssetProps & Omit<React.SVGProps<SVGSVGElement>, "x" | "y" | "width" | "height" | "viewBox" | "xmlns">

const Silver: React.FC<SilverProps> = ({ x, y, width, height, ...props }) => {
    return (
        <svg {...props} x={x} y={y} width={width} height={height} viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
            <rect x="192" y="128" width="64" height="64" fill="#434343" />
            <rect x="256" y="128" width="64" height="64" fill="#434343" />
            <rect x="128" y="192" width="64" height="64" fill="#434343" />
            <rect x="192" y="192" width="64" height="64" fill="#434343" />
            <rect x="256" y="192" width="64" height="64" fill="#434343" />
            <rect x="320" y="192" width="64" height="64" fill="#434343" />
            <rect x="64" y="256" width="64" height="64" fill="#434343" />
            <rect x="128" y="256" width="64" height="64" fill="#434343" />
            <rect x="192" y="256" width="64" height="64" fill="#434343" />
            <rect x="256" y="256" width="64" height="64" fill="#CEFFF7" />
            <rect x="320" y="256" width="64" height="64" fill="#434343" />
            <rect x="384" y="256" width="64" height="64" fill="#434343" />
            <rect x="64" y="320" width="64" height="64" fill="#434343" />
            <rect x="128" y="320" width="64" height="64" fill="#434343" />
            <rect x="192" y="320" width="64" height="64" fill="#CEFFF7" />
            <rect x="256" y="320" width="64" height="64" fill="#434343" />
            <rect x="320" y="320" width="64" height="64" fill="#434343" />
            <rect x="128" y="384" width="64" height="64" fill="#434343" />
            <rect x="192" y="384" width="64" height="64" fill="#434343" />
            <rect x="256" y="384" width="64" height="64" fill="#434343" />
        </svg>
    )
}

export default Silver
