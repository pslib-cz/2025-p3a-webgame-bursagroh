import React from "react"
import type { AssetProps } from "../../../types"

type ZombieProps = AssetProps & Omit<React.SVGProps<SVGSVGElement>, "x" | "y" | "width" | "height" | "viewBox" | "xmlns">

const Zombie: React.FC<ZombieProps> = ({ x, y, width, height, ...props }) => {
    return (
        <svg {...props} x={x} y={y} width={width} height={height} viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
            <rect x="128" y="64" width="64" height="64" fill="#132A13" />
            <rect x="192" y="64" width="64" height="64" fill="#132A13" />
            <rect x="256" y="64" width="64" height="64" fill="#132A13" />
            <rect x="320" y="64" width="64" height="64" fill="#132A13" />
            <rect x="128" y="128" width="64" height="64" fill="#132A13" />
            <rect x="192" y="128" width="64" height="64" fill="black" />
            <rect x="256" y="128" width="64" height="64" fill="#132A13" />
            <rect x="320" y="128" width="64" height="64" fill="black" />
            <rect x="128" y="192" width="64" height="64" fill="#132A13" />
            <rect x="192" y="192" width="64" height="64" fill="#132A13" />
            <rect x="256" y="192" width="64" height="64" fill="#132A13" />
            <rect x="320" y="192" width="64" height="64" fill="#132A13" />
            <rect x="320" y="256" width="64" height="64" fill="#4F772D" />
            <rect x="320" y="320" width="64" height="64" fill="#4F772D" />
            <rect x="384" y="320" width="64" height="64" fill="#4F772D" />
            <rect x="192" y="384" width="64" height="64" fill="#364085" />
            <rect x="256" y="384" width="64" height="64" fill="#364085" />
            <rect x="320" y="384" width="64" height="64" fill="#364085" />
            <rect x="384" y="384" width="64" height="64" fill="#364085" />
            <rect x="192" y="448" width="64" height="64" fill="#364085" />
            <rect x="384" y="448" width="64" height="64" fill="#364085" />
            <rect x="128" y="256" width="64" height="64" fill="#4F772D" />
            <rect x="192" y="256" width="64" height="64" fill="#4F772D" />
            <rect x="256" y="256" width="64" height="64" fill="#4F772D" />
            <rect x="192" y="320" width="64" height="64" fill="#4F772D" />
            <rect x="256" y="320" width="64" height="64" fill="#4F772D" />
        </svg>
    )
}

export default Zombie
