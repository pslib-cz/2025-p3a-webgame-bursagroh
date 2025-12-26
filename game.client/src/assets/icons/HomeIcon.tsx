import React from 'react'

const HomeIcon: React.FC<React.SVGProps<SVGSVGElement>> = ({ viewBox = "0 0 24 24", fill = "black", xmlns = "http://www.w3.org/2000/svg", ...props }) => {
    return (
        <svg {...props} viewBox={viewBox} fill={fill} xmlns={xmlns}>
            <g clipPath="url(#clip0_3368_60)">
                <path d="M10 20V14H14V20H19V12H22L12 3L2 12H5V20H10Z" />
            </g>
            <defs>
                <clipPath id="clip0_3368_60">
                    <rect width="24" height="24" />
                </clipPath>
            </defs>
        </svg>
    )
}

export default HomeIcon