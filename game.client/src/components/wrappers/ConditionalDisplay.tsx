import React from 'react'

type ConditionalDisplayProps = {
    condition: boolean
}

const ConditionalDisplay: React.FC<React.PropsWithChildren<ConditionalDisplayProps>> = ({ condition, children }) => {
    if (!condition) {
        return null
    }

    return children
}

export default ConditionalDisplay