import React from "react"
import type { AssetProps } from "../../../types"

type FloorWallProps = {rotation: "0deg" | "90deg" | "180deg" | "270deg"} & AssetProps & Omit<React.SVGProps<SVGSVGElement>, "x" | "y" | "width" | "height" | "viewBox" | "xmlns">

const FloorWall: React.FC<FloorWallProps> = ({ x, y, width, height, rotation, ...props }) => {
    return (
        <svg {...props} x={x} y={y} width={width} height={height} viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg" style={{rotate: rotation, transformOrigin: `${x + width / 2}px ${y + height / 2}px`}}>
            <rect x="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="128" width="64" height="64" fill="#A1A0A0" />
            <rect x="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="384" width="64" height="64" fill="#A1A0A0" />
            <rect x="64" y="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="128" y="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="192" y="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="256" y="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="320" y="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="384" y="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="64" y="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="128" y="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="192" y="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="256" y="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="320" y="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="384" y="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="64" y="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="128" y="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="192" y="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="256" y="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="320" y="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="384" y="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="64" y="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="128" y="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="192" y="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="320" y="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="384" y="320" width="64" height="64" fill="#A1A0A0" />
            <rect y="384" width="64" height="64" fill="#D9D9D9" />
            <rect x="64" y="384" width="64" height="64" fill="#D9D9D9" />
            <rect x="128" y="384" width="64" height="64" fill="#D9D9D9" />
            <rect x="320" y="384" width="64" height="64" fill="#D9D9D9" />
            <rect x="384" y="384" width="64" height="64" fill="#D9D9D9" />
            <rect y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="64" y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="128" y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="192" y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="256" y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="320" y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="384" y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="64" y="128" width="64" height="64" fill="#A1A0A0" />
            <rect x="128" y="128" width="64" height="64" fill="#A1A0A0" />
            <rect x="192" y="128" width="64" height="64" fill="#A1A0A0" />
            <rect x="256" y="128" width="64" height="64" fill="#A1A0A0" />
            <rect x="320" y="128" width="64" height="64" fill="#A1A0A0" />
            <rect x="384" y="128" width="64" height="64" fill="#A1A0A0" />
            <rect width="64" height="64" fill="#A1A0A0" />
            <rect x="448" width="64" height="64" fill="#A1A0A0" />
            <rect y="64" width="64" height="64" fill="#A1A0A0" />
            <rect x="448" y="64" width="64" height="64" fill="#A1A0A0" />
            <rect y="128" width="64" height="64" fill="#A1A0A0" />
            <rect x="448" y="128" width="64" height="64" fill="#A1A0A0" />
            <rect y="192" width="64" height="64" fill="#A1A0A0" />
            <rect x="448" y="192" width="64" height="64" fill="#A1A0A0" />
            <rect y="256" width="64" height="64" fill="#A1A0A0" />
            <rect x="448" y="256" width="64" height="64" fill="#A1A0A0" />
            <rect y="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="448" y="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="448" y="384" width="64" height="64" fill="#D9D9D9" />
            <rect x="448" y="448" width="64" height="64" fill="#D9D9D9" />
            <rect x="256" y="320" width="64" height="64" fill="#A1A0A0" />
            <rect x="192" y="384" width="64" height="64" fill="#D9D9D9" />
            <rect x="256" y="384" width="64" height="64" fill="#D9D9D9" />
        </svg>
    )
}

export default FloorWall
