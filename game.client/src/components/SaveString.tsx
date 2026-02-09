import React from 'react'
import CopyIcon from '../icons/CopyIcon'
import styles from './saveString.module.css'
import Text from './Text'

type SaveStringProps = {
    saveString: string
    onFinished?: () => void
}

const SaveString: React.FC<SaveStringProps> = ({ saveString, onFinished }) => {
    const [isExiting, setIsExiting] = React.useState(false)

    React.useEffect(() => {
        if (onFinished === undefined) return

        const exitTimer = setTimeout(() => setIsExiting(true), 9000)
        const totalTimer = setTimeout(() => onFinished(), 9500) 

        return () => {
            clearTimeout(exitTimer)
            clearTimeout(totalTimer)
        }
    }, [onFinished])

    const handleCopy = () => {
        navigator.clipboard.writeText(saveString)
    }

    return (
        <div className={`${styles.container} ${onFinished === undefined ? '' : styles.containerAnimated} ${isExiting ? styles.exit : ''}`}>
            <Text size="h3">{saveString}</Text>
            <CopyIcon className={styles.copy} onClick={handleCopy} />
        </div>
    )
}

export default SaveString
