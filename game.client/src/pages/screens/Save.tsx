import React from 'react'
import Layer from '../../components/wrappers/layer/Layer'
import { SaveContext } from '../../providers/SaveProvider'
import Link from '../../components/Link'
import SaveString from '../../components/SaveString'
import useBlur from '../../hooks/useBlur'
import styles from './save.module.css'
import { useNavigate } from 'react-router'
import useKeyboard from '../../hooks/useKeyboard'

const SaveScreen = () => {
    useBlur(true)

    const navigate = useNavigate()
    
    const saveString = React.useContext(SaveContext)!.saveString!

    useKeyboard("Escape", () => {
        navigate("/")
    })

    return (
        <Layer layer={1}>
            <div className={styles.container}>
                <div className={styles.subContainer}>
                    <span className={styles.heading}>Save</span>
                    <SaveString saveString={saveString} />
                </div>
                <Link to='/'>Back</Link>
            </div>
        </Layer>
    )
}

export default SaveScreen