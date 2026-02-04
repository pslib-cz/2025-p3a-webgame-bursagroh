import React from 'react'
import styles from './button.module.css'

type ButtonProps = {
    onClick?: () => void
    disabled?: boolean
    isDangerous?: boolean
} & React.PropsWithChildren

const Button: React.FC<ButtonProps> = ({ onClick, disabled, children, isDangerous }) => {   
    return (
        <button onClick={onClick} className={`${isDangerous ? styles.dangerous : ''} ${disabled ? styles.disabledButton : styles.button}`} disabled={disabled}>{children}</button>
    )
}

export default Button