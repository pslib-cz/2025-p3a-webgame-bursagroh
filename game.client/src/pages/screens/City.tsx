import useBlur from "../../hooks/useBlur"
import useMap from "../../hooks/useMap"

const CityScreen = () => {
    useBlur(false)
    useMap("city")
    
    return null
}

export default CityScreen
