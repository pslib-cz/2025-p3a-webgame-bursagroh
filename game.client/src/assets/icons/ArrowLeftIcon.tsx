import React from 'react'

const ArrowLeftIcon: React.FC<React.SVGProps<SVGSVGElement>> = ({ viewBox = "0 0 24 24", fill = "black", xmlns = "http://www.w3.org/2000/svg", ...props }) => {
    return (
        <svg {...props} viewBox={viewBox} fill={fill} xmlns={xmlns}>
            <g clipPath="url(#clip0_3371_16993)">
                <path d="M15.41 16.59L10.83 12L15.41 7.41L14 6L8 12L14 18L15.41 16.59Z" />
            </g>
            <defs>
                <clipPath id="clip0_3371_16993">
                    <rect width="24" height="24" />
                </clipPath>
            </defs>
        </svg>
    )
}

export default ArrowLeftIcon