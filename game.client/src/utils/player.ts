export const validMove = (playerX: number, playerY: number, newX: number, newY: number): boolean => {
    const deltaX = Math.abs(newX - playerX)
    const deltaY = Math.abs(newY - playerY)
    
    return (deltaX + deltaY) === 1
}