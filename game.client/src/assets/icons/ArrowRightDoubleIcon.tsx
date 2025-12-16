import React from 'react'

const ArrowRightDoubleIcon: React.FC<React.SVGProps<SVGSVGElement>> = ({ viewBox = "0 0 24 24", fill = "black", xmlns = "http://www.w3.org/2000/svg", ...props }) => {
    return (
        <svg {...props} viewBox={viewBox} fill={fill} xmlns={xmlns}>
            <g clipPath="url(#clip0_3371_16992)">
                <path d="M6.41 6L5 7.41L9.58 12L5 16.59L6.41 18L12.41 12L6.41 6Z" />
                <path d="M13 6L11.59 7.41L16.17 12L11.59 16.59L13 18L19 12L13 6Z" />
            </g>
            <defs>
                <clipPath id="clip0_3371_16992">
                    <rect width="24" height="24" />
                </clipPath>
            </defs>
        </svg>
    )
}

export default ArrowRightDoubleIcon