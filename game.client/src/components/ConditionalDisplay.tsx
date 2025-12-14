import React from 'react'

type ConditionalDisplayProps = {
    visible: boolean
} & React.PropsWithChildren

const ConditionalDisplay: React.FC<ConditionalDisplayProps> = ({visible, children}) => {
    if (visible) {
        return <>{children}</>
    }
}

export default ConditionalDisplay