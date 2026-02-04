import React from 'react'
import Layer from '../../components/wrappers/layer/Layer'
import Link from '../../components/Link'
import Input from '../../components/Input'
import { SaveContext } from '../../providers/SaveProvider'
import { parseSave } from '../../utils/save'

const LoadScreen = () => {
    const [userSaveString, setUserSaveString] = React.useState("")

    const saves = React.useContext(SaveContext)!.saves

    return (
        <Layer layer={1}>
            <div>
                <span>Load</span>
                <div>
                    {saves.autosaves.map((save, index) => (
                        <Link key={`autosave_${index}`} to={`/load/${encodeURIComponent(save.saveString)}`}>{parseSave(save, true)}</Link>
                    ))}
                    {saves.saves.map((save, index) => (
                        <Link key={`save_${index}`} to={`/load/${encodeURIComponent(save.saveString)}`}>{parseSave(save)}</Link>
                    ))}
                    <Input placeholder="SaveString" value={userSaveString} onChange={e => setUserSaveString(e.target.value)} />
                    <Link to={`/load/${encodeURIComponent(userSaveString)}`}>Load</Link>
                </div>
                <Link to="/">Back</Link>
            </div>
        </Layer>
    )
}

export default LoadScreen