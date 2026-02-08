import React from 'react'
import useMove from './useMove'
import { PlayerContext } from '../providers/global/PlayerProvider'
import useKeyboard from './useKeyboard'

type Direction = "up" | "down" | "left" | "right"

const getTargetPosition = (direction: Direction, positionX: number, positionY: number) => {
    switch (direction) {
        case "up":
            return { x: positionX, y: positionY - 1 }
        case "down":
            return { x: positionX, y: positionY + 1 }
        case "left":
            return { x: positionX - 1, y: positionY }
        case "right":
            return { x: positionX + 1, y: positionY }
    }
}

const useKeyboardMove = (isSubMove: boolean) => {
    const move = useMove()

    const player = React.useContext(PlayerContext)!.player!

    const handleMove = async (direction: Direction) => {
        const targetPosition = getTargetPosition(direction, isSubMove ? player.subPositionX : player.positionX, isSubMove ? player.subPositionY : player.positionY)
        await move(targetPosition.x, targetPosition.y, isSubMove)
    }

    useKeyboard("ArrowUp", async () => {
        await handleMove("up")
    })

    useKeyboard("ArrowDown", async () => {
        await handleMove("down")
    })

    useKeyboard("ArrowLeft", async () => {
        await handleMove("left")
    })

    useKeyboard("ArrowRight", async () => {
        await handleMove("right")
    })

    useKeyboard("w", async () => {
        await handleMove("up")
    })

    useKeyboard("s", async () => {
        await handleMove("down")
    })

    useKeyboard("a", async () => {
        await handleMove("left")
    })

    useKeyboard("d", async () => {
        await handleMove("right")
    })
}

export default useKeyboardMove