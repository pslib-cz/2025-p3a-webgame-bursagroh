import useBlur from "../../hooks/useBlur"
import useKeyboard from "../../hooks/useKeyboard"
import useMap from "../../hooks/useMap"
import useKeyboardMove from "../../hooks/useKeyboardMove"
import useLink from "../../hooks/useLink"

const CityScreen = () => {
    useBlur(false)
    useMap("city")
    useKeyboardMove(false)

    const moveToPage = useLink()

    useKeyboard("Escape", async () => {
        await moveToPage("root")
    })

    return null
}

export default CityScreen
