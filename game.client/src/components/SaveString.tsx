import { useState, useEffect } from 'react'
import CopyIcon from '../icons/CopyIcon'
import styles from './saveString.module.css'
import Text from './Text'

const SaveString = ({ saveString, onFinished }: { saveString: string, onFinished: () => void }) => {
    const [isExiting, setIsExiting] = useState(false)

    useEffect(() => {
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
        <div className={`${styles.container} ${isExiting ? styles.exit : ''}`}>
            <Text size="h3">{saveString}</Text>
            <CopyIcon className={styles.copy} width={32} height={32} onClick={handleCopy} />
        </div>
    )
}

export default SaveString
