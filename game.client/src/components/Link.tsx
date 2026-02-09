import React from 'react'
import styles from './link.module.css'
import useLink from '../hooks/useLink'
import { type PageType } from '../types/page'
import Text from './Text'

type LinkProps = {
    to: PageType
    onClick?: () => Promise<void> | void
    disabled?: boolean
    moveScreen?: boolean
    saveString?: string
} & React.PropsWithChildren

const Link: React.FC<LinkProps> = ({ to, onClick, disabled, children, moveScreen, saveString }) => {
    const moveToPage = useLink()

    const handleClick = async () => {
        if (onClick) {
            await onClick()
        }

        await moveToPage(to, moveScreen, saveString)
    }

    if (disabled) {
        return (
            <Text size="h2" className={styles.disabledLink}>{children}</Text>
        )
    }
    
    return (
        <Text size="h2" onClick={handleClick} className={styles.link}>{children}</Text>
    )
}

export default Link