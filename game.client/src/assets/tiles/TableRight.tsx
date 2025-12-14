import React from "react"
import type { AssetProps } from "../../types"

type TableRightProps = AssetProps & Omit<React.SVGProps<SVGSVGElement>, "x" | "y" | "width" | "height" | "viewBox" | "xmlns">

const TableRight: React.FC<TableRightProps> = ({ x, y, width, height, ...props }) => {
    return (
        <svg {...props} x={x} y={y} width={width} height={height} viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
            <rect x="256" width="64" height="64" fill="#B7B7B7" />
            <rect y="128" width="64" height="64" fill="#53300E" />
            <rect x="64" y="128" width="64" height="64" fill="#53300E" />
            <rect x="128" y="128" width="64" height="64" fill="#53300E" />
            <rect x="192" y="128" width="64" height="64" fill="#53300E" />
            <rect x="256" y="128" width="64" height="64" fill="#53300E" />
            <rect y="320" width="64" height="64" fill="#53300E" />
            <rect x="64" y="320" width="64" height="64" fill="#53300E" />
            <rect x="128" y="320" width="64" height="64" fill="#53300E" />
            <rect x="192" y="320" width="64" height="64" fill="#53300E" />
            <rect x="256" y="320" width="64" height="64" fill="#53300E" />
            <rect x="320" y="320" width="64" height="64" fill="#53300E" />
            <rect x="384" y="320" width="64" height="64" fill="#53300E" />
            <rect y="384" width="64" height="64" fill="#53300E" />
            <rect x="64" y="384" width="64" height="64" fill="#53300E" />
            <rect x="128" y="384" width="64" height="64" fill="#53300E" />
            <rect x="192" y="384" width="64" height="64" fill="#53300E" />
            <rect x="256" y="384" width="64" height="64" fill="#53300E" />
            <rect x="320" y="384" width="64" height="64" fill="#53300E" />
            <rect x="384" y="384" width="64" height="64" fill="#53300E" />
            <rect x="448" y="384" width="64" height="64" fill="#53300E" />
            <rect x="256" y="448" width="64" height="64" fill="#53300E" />
            <rect x="320" y="448" width="64" height="64" fill="#53300E" />
            <rect x="320" width="64" height="64" fill="#B7B7B7" />
            <rect x="320" y="64" width="64" height="64" fill="#B7B7B7" />
            <rect x="384" y="64" width="64" height="64" fill="#B7B7B7" />
            <rect x="320" y="128" width="64" height="64" fill="#B7B7B7" />
            <rect x="384" y="128" width="64" height="64" fill="#B7B7B7" />
            <rect x="320" y="192" width="64" height="64" fill="#B7B7B7" />
            <rect x="384" y="192" width="64" height="64" fill="#B7B7B7" />
            <rect x="256" y="256" width="64" height="64" fill="#B7B7B7" />
            <rect x="320" y="256" width="64" height="64" fill="#B7B7B7" />
        </svg>
    )
}

export default TableRight
