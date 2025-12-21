import React from 'react'

const ArrowRightIcon: React.FC<React.SVGProps<SVGSVGElement>> = ({ viewBox = "0 0 24 24", fill = "black", xmlns = "http://www.w3.org/2000/svg", ...props }) => {
    return (
        <svg {...props} viewBox={viewBox} fill={fill} xmlns={xmlns}>
            <g clipPath="url(#clip0_3371_16991)">
                <path d="M8.59 16.59L13.17 12L8.59 7.41L10 6L16 12L10 18L8.59 16.59Z" />
            </g>
            <defs>
                <clipPath id="clip0_3371_16991">
                    <rect width="24" height="24" />
                </clipPath>
            </defs>
        </svg>
    )
}

export default ArrowRightIcon