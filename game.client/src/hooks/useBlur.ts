import React from 'react'
import { IsBluredContext } from '../providers/global/IsBluredProvider'

const useBlur = (isBlured: boolean) => {
    const setIsBlured = React.useContext(IsBluredContext)!.setIsBlured
    
    React.useEffect(() => {
        setIsBlured(isBlured)
    }, [isBlured, setIsBlured])
}

export default useBlur