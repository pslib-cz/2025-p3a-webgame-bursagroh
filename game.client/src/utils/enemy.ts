import type { EnemyType as APIEnemyType } from "../types/api/models/building";
import type { EnemyType } from "../types/enemy";


export const mapEnemyType = (enemyType: APIEnemyType): EnemyType => {
    switch (enemyType) {
        case 'Zombie':
            return 'zombie'
        case 'Skeleton':
            return 'skeleton'
        case 'Dragon':
            return 'dragon'
    }
}
