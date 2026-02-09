import React from 'react'
import styles from './text.module.css'
import type { TextSize } from '../types/text'

type TextProps = {
    size: TextSize
} & React.HTMLAttributes<HTMLSpanElement>

const Text: React.FC<React.PropsWithChildren<TextProps>> = ({ size, children, className, ...props }) => {
    switch (size) {
        case 'h0':
            return (
                <span className={`${styles.text} ${styles.h0} ${className || ''}`} {...props}>{children}</span>
            )
        case 'h1':
            return (
                <span className={`${styles.text} ${styles.h1} ${className || ''}`} {...props}>{children}</span>
            )
        case 'h2':
            return (
                <span className={`${styles.text} ${styles.h2} ${className || ''}`} {...props}>{children}</span>
            )
        case 'h3':
            return (
                <span className={`${styles.text} ${styles.h3} ${className || ''}`} {...props}>{children}</span>
            )
        case 'h4':
            return (
                <span className={`${styles.text} ${styles.h4} ${className || ''}`} {...props}>{children}</span>
            )
        case 'h5':
            return (
                <span className={`${styles.text} ${styles.h5} ${className || ''}`} {...props}>{children}</span>
            )
    }
}

export default Text