import React from 'react'

type ConditionalDisplayProps = {
    condition: boolean
    notMet?: React.ReactNode
}

const ConditionalDisplay: React.FC<React.PropsWithChildren<ConditionalDisplayProps>> = ({ condition, children, notMet }) => {
    if (!condition) {
        return notMet
    }

    return children
}

export default ConditionalDisplay