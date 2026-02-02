import { type JSX } from 'react'

type ArrayDisplayProps = {
    elements: JSX.Element[]
    ifEmpty: JSX.Element
}

const ArrayDisplay: React.FC<ArrayDisplayProps> = ({ elements, ifEmpty }) => {
    if (elements.length === 0) {
        return ifEmpty
    }

    return elements
}

export default ArrayDisplay