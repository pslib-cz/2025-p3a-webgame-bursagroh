import React from 'react'
import { NavLink } from 'react-router'
import styles from './link.module.css'

type LinkProps = {
    to: string
    onClick?: () => void
    disabled?: boolean
} & React.PropsWithChildren

const Link: React.FC<LinkProps> = ({ to, onClick, disabled, children }) => {
    if (disabled) {
        return (
            <span className={styles.disabledLink}>{children}</span>
        )
    }
    
    return (
        <NavLink to={to} onClick={onClick} className={styles.link}>{children}</NavLink>
    )
}

export default Link