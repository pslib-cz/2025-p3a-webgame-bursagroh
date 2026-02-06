import React from 'react'
import Layer from '../../components/wrappers/layer/Layer'
import { SaveContext } from '../../providers/SaveProvider'
import Link from '../../components/Link'
import SaveString from '../../components/SaveString'
import useBlur from '../../hooks/useBlur'
import styles from './save.module.css'

const SaveScreen = () => {
    useBlur(true)
    
    const saveString = React.useContext(SaveContext)!.saveString!

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