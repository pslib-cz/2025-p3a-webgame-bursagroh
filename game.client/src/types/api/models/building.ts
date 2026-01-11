export type BuildingType = "Fountain" | "Bank" | "Restaurant" | "Mine" | "Blacksmith" | "Abandoned" | "AbandonedTrap" | "Road"
export type FloorItemType = "Stair" | "Chest" | "Item" | "Enemy"
export type EnemyType = "Zombie" | "Skeleton" | "Dragon"

export type Building = {
    buildingId: number
    playerId: string
    positionX: number
    positionY: number
} & (
    | {
          buildingType: "Fountain" | "Bank" | "Restaurant" | "Mine" | "Blacksmith" | "Road"
      }
    | {
          buildingType: "Abandoned" | "AbandonedTrap"
          height: number
          reachedHeight: number
          isBossDefeated: boolean
          floors: Array<Floor>
      }
)

export type Floor = {
    floorId: number
    buildingId: number
    level: number
    floorItems: Array<FloorItem>
}

export type FloorItem = {
    floorItemId: number
    floorId: number
    positionX: number
    positionY: number
    floorItemType: FloorItemType
    enemy: Enemy | null
}

export type Enemy = {
    enemyId: number,
    health: number,
    enemyType: EnemyType,
    floorItemId: number,
    itemInstanceId: number
}