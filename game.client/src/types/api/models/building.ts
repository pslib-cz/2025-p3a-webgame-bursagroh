export type BuildingType = 0 | 1 | 2 | 3 | 4

export type Building = {
    "buildingId": number,
    "playerId": string,
    "positionX": number,
    "positionY": number,
    "buildingType": BuildingType,
  } 
  
  {
    "height": number | null,
    "reachedHeight": number | null,
    "isBossDefeated": boolean | null,
    "floors": [
      {
        "floorId": 1,
        "buildingId": 1,
        "level": 1,
        "floorItems": [
          {
            "floorItemId": 1,
            "floorId": 1,
            "positionX": 1,
            "positionY": 1,
            "floorItemType": "Stair"
          }
        ]
      }
    ]
  } | {
    "height": number | null,
    "reachedHeight": number | null,
    "isBossDefeated": boolean | null,
    "floors": [
      {
        "floorId": 1,
        "buildingId": 1,
        "level": 1,
        "floorItems": [
          {
            "floorItemId": 1,
            "floorId": 1,
            "positionX": 1,
            "positionY": 1,
            "floorItemType": "Stair"
          }
        ]
      }
    ]
  }