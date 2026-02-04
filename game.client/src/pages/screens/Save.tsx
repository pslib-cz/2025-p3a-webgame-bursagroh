import React from 'react'
import Layer from '../../components/wrappers/layer/Layer'
import { SaveContext } from '../../providers/SaveProvider'
import Link from '../../components/Link'
import SaveString from '../../components/SaveString'
import useBlur from '../../hooks/useBlur'

const SaveScreen = () => {
    useBlur(true)
    
    const saveString = React.useContext(SaveContext)!.saveString!

    return (
        <Layer layer={1}>
            <div>
                <span>Save</span>
                <SaveString saveString={saveString} />
                <Link to='/'>Back</Link>
            </div>
        </Layer>
    )
}

export default SaveScreen