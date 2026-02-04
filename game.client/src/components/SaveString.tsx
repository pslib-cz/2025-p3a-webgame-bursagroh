import React from 'react'
import CopyIcon from '../assets/icons/CopyIcon'
import styles from './saveString.module.css'

type SaveStringProps = {
    saveString: string
}

const SaveString: React.FC<SaveStringProps> = ({ saveString }) => {
    const handleCopy = () => {
        navigator.clipboard.writeText(saveString)
    }

    return (
        <div className={styles.container}>
            <span className={styles.text}>{saveString}</span>
            <CopyIcon className={styles.copy} width={32} height={32} onClick={handleCopy} />
        </div>
    )
}

export default SaveString