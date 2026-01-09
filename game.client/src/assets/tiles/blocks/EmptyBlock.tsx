import React from 'react'
import type { AssetProps } from "../../../types"

type EmptyBlockProps = AssetProps & Omit<React.SVGProps<SVGSVGElement>, "x" | "y" | "width" | "height" | "viewBox" | "xmlns">

const EmptyBlock: React.FC<EmptyBlockProps> = ({x, y, width, height, ...props}) => {
    return (
        <svg {...props} x={x} y={y} width={width} height={height} viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
            <rect width="64" height="64" fill="#636363ff" />
            <rect x="64" width="64" height="64" fill="#636363ff" />
            <rect x="128" width="64" height="64" fill="#636363ff" />
            <rect x="192" width="64" height="64" fill="#636363ff" />
            <rect x="256" width="64" height="64" fill="#636363ff" />
            <rect x="320" width="64" height="64" fill="#636363ff" />
            <rect x="384" width="64" height="64" fill="#636363ff" />
            <rect x="448" width="64" height="64" fill="#636363ff" />
            <rect y="64" width="64" height="64" fill="#636363ff" />
            <rect x="64" y="64" width="64" height="64" fill="#636363ff" />
            <rect x="128" y="64" width="64" height="64" fill="#636363ff" />
            <rect x="192" y="64" width="64" height="64" fill="#636363ff" />
            <rect x="256" y="64" width="64" height="64" fill="#636363ff" />
            <rect x="320" y="64" width="64" height="64" fill="#636363ff" />
            <rect x="384" y="64" width="64" height="64" fill="#636363ff" />
            <rect x="448" y="64" width="64" height="64" fill="#636363ff" />
            <rect y="128" width="64" height="64" fill="#636363ff" />
            <rect x="64" y="128" width="64" height="64" fill="#636363ff" />
            <rect x="128" y="128" width="64" height="64" fill="#636363ff" />
            <rect x="192" y="128" width="64" height="64" fill="#636363ff" />
            <rect x="256" y="128" width="64" height="64" fill="#636363ff" />
            <rect x="320" y="128" width="64" height="64" fill="#636363ff" />
            <rect x="384" y="128" width="64" height="64" fill="#636363ff" />
            <rect x="448" y="128" width="64" height="64" fill="#636363ff" />
            <rect y="192" width="64" height="64" fill="#636363ff" />
            <rect x="64" y="192" width="64" height="64" fill="#636363ff" />
            <rect x="128" y="192" width="64" height="64" fill="#636363ff" />
            <rect x="192" y="192" width="64" height="64" fill="#636363ff" />
            <rect x="256" y="192" width="64" height="64" fill="#636363ff" />
            <rect x="320" y="192" width="64" height="64" fill="#636363ff" />
            <rect x="384" y="192" width="64" height="64" fill="#636363ff" />
            <rect x="448" y="192" width="64" height="64" fill="#636363ff" />
            <rect y="256" width="64" height="64" fill="#636363ff" />
            <rect x="64" y="256" width="64" height="64" fill="#636363ff" />
            <rect x="128" y="256" width="64" height="64" fill="#636363ff" />
            <rect x="192" y="256" width="64" height="64" fill="#636363ff" />
            <rect x="256" y="256" width="64" height="64" fill="#636363ff" />
            <rect x="320" y="256" width="64" height="64" fill="#636363ff" />
            <rect x="384" y="256" width="64" height="64" fill="#636363ff" />
            <rect x="448" y="256" width="64" height="64" fill="#636363ff" />
            <rect y="320" width="64" height="64" fill="#636363ff" />
            <rect x="64" y="320" width="64" height="64" fill="#636363ff" />
            <rect x="128" y="320" width="64" height="64" fill="#636363ff" />
            <rect x="192" y="320" width="64" height="64" fill="#636363ff" />
            <rect x="256" y="320" width="64" height="64" fill="#636363ff" />
            <rect x="320" y="320" width="64" height="64" fill="#636363ff" />
            <rect x="384" y="320" width="64" height="64" fill="#636363ff" />
            <rect x="448" y="320" width="64" height="64" fill="#636363ff" />
            <rect y="384" width="64" height="64" fill="#636363ff" />
            <rect x="64" y="384" width="64" height="64" fill="#636363ff" />
            <rect x="128" y="384" width="64" height="64" fill="#636363ff" />
            <rect x="192" y="384" width="64" height="64" fill="#636363ff" />
            <rect x="256" y="384" width="64" height="64" fill="#636363ff" />
            <rect x="320" y="384" width="64" height="64" fill="#636363ff" />
            <rect x="384" y="384" width="64" height="64" fill="#636363ff" />
            <rect x="448" y="384" width="64" height="64" fill="#636363ff" />
            <rect y="448" width="64" height="64" fill="#636363ff" />
            <rect x="64" y="448" width="64" height="64" fill="#636363ff" />
            <rect x="128" y="448" width="64" height="64" fill="#636363ff" />
            <rect x="192" y="448" width="64" height="64" fill="#636363ff" />
            <rect x="256" y="448" width="64" height="64" fill="#636363ff" />
            <rect x="320" y="448" width="64" height="64" fill="#636363ff" />
            <rect x="384" y="448" width="64" height="64" fill="#636363ff" />
            <rect x="448" y="448" width="64" height="64" fill="#636363ff" />
        </svg>
  )
}

export default EmptyBlock