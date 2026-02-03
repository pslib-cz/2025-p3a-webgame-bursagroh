import React from 'react'
import { assetTypeToId } from '../../utils/asset'

const Salad: React.FC = () => {
    return (
        <svg id={assetTypeToId("salad")} x={0} y={0} width="100%" height="100%" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
            <rect x="128" y="256" width="64" height="64" fill="#3BFF00" />
            <rect x="192" y="256" width="64" height="64" fill="#3BFF00" />
            <rect x="256" y="256" width="64" height="64" fill="#3BFF00" />
            <rect x="64" y="320" width="64" height="64" fill="#3BFF00" />
            <rect x="128" y="320" width="64" height="64" fill="#3BFF00" />
            <rect x="192" y="320" width="64" height="64" fill="#3BFF00" />
            <rect x="256" y="320" width="64" height="64" fill="#3BFF00" />
            <rect x="320" y="320" width="64" height="64" fill="#3BFF00" />
            <rect x="128" y="384" width="64" height="64" fill="#3BFF00" />
            <rect x="192" y="384" width="64" height="64" fill="#3BFF00" />
            <rect x="256" y="384" width="64" height="64" fill="#3BFF00" />
            <rect x="320" y="384" width="64" height="64" fill="#3BFF00" />
            <rect x="192" y="448" width="64" height="64" fill="#3BFF00" />
            <rect x="256" y="448" width="64" height="64" fill="#3BFF00" />
        </svg>
    )
}

export default Salad