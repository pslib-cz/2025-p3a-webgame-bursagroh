import React from 'react'
import type { Recipe } from '../types/api/models/recipe'

type BurgerProps = {
    burger: Recipe
}

const Burger: React.FC<BurgerProps> = ({ burger }) => {
  return (
    <div>
        {burger.ingrediences.sort((a, b) => a.order - b.order).map((ingredience, index) => (
            <span key={index}>
                {ingredience.order}. {ingredience.ingredienceType}
            </span>
        ))}
        <span>{burger.name}</span>
    </div>
  )
}

export default Burger