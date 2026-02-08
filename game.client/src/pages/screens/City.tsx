import { useNavigate } from "react-router"
import useBlur from "../../hooks/useBlur"
import useKeyboard from "../../hooks/useKeyboard"
import useMap from "../../hooks/useMap"
import useKeyboardMove from "../../hooks/useKeyboardMove"

const CityScreen = () => {
    useBlur(false)
    useMap("city")
    useKeyboardMove(false)

    const navigate = useNavigate()

    useKeyboard("Escape", () => {
        navigate("/")
    })

    return null
}

export default CityScreen
