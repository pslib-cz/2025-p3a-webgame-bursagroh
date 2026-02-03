import Bacon from '../../assets/burger/Bacon'
import BunDown from '../../assets/burger/BunDown'
import BunUp from '../../assets/burger/BunUp'
import Cheese from '../../assets/burger/Cheese'
import Meat from '../../assets/burger/Meat'
import Salad from '../../assets/burger/Salad'
import Sauce from '../../assets/burger/Sauce'
import Tomato from '../../assets/burger/Tomato'
import Copper from '../../assets/items/Copper'
import Gold from '../../assets/items/Gold'
import Iron from '../../assets/items/Iron'
import WoodenPickaxe from '../../assets/items/pickaxe/WoodenPickaxe'
import RockItem from '../../assets/items/RockItem'
import Silver from '../../assets/items/Silver'
import WoodenSword from '../../assets/items/sword/WoodenSword'
import Unobtanium from '../../assets/items/Unobtanium'
import Wood from '../../assets/items/Wood'
import Player from '../../assets/Player'
import CopperOre from '../../assets/tiles/blocks/CopperOre'
import EmptyBlock from '../../assets/tiles/blocks/EmptyBlock'
import GoldOre from '../../assets/tiles/blocks/GoldOre'
import IronOre from '../../assets/tiles/blocks/IronOre'
import Rock from '../../assets/tiles/blocks/Rock'
import SilverOre from '../../assets/tiles/blocks/SilverOre'
import UnobtainiumOre from '../../assets/tiles/blocks/UnobtainiumOre'
import WoodenFrame from '../../assets/tiles/blocks/WoodenFrame'
import Bank from '../../assets/tiles/buildings/Bank'
import Blacksmith from '../../assets/tiles/buildings/Blacksmith'
import BuildingCorner from '../../assets/tiles/buildings/building/BuildingCorner'
import BuildingCornerTrap from '../../assets/tiles/buildings/building/BuildingCornerTrap'
import BuildingStraight from '../../assets/tiles/buildings/building/BuildingStraight'
import BuildingStraightTrap from '../../assets/tiles/buildings/building/BuildingStraightTrap'
import Fountain from '../../assets/tiles/buildings/Fountain'
import Grass from '../../assets/tiles/buildings/Grass'
import Mine from '../../assets/tiles/buildings/Mine'
import Restaurant from '../../assets/tiles/buildings/Restaurant'
import JunctionRoad from '../../assets/tiles/buildings/road/JunctionRoad'
import StraightRoad from '../../assets/tiles/buildings/road/StraightRoad'
import Dragon from '../../assets/tiles/enemies/Dragon'
import Skeleton from '../../assets/tiles/enemies/Skeleton'
import Zombie from '../../assets/tiles/enemies/Zombie'
import Floor from '../../assets/tiles/floors/Floor'
import FloorCorner from '../../assets/tiles/floors/FloorCorner'
import FloorDoorLeft from '../../assets/tiles/floors/FloorDoorLeft'
import FloorDoorRight from '../../assets/tiles/floors/FloorDoorRight'
import FloorStairs from '../../assets/tiles/floors/FloorStairs'
import FloorWall from '../../assets/tiles/floors/FloorWall'
import TableLeft from '../../assets/tiles/TableLeft'
import TableRight from '../../assets/tiles/TableRight'

const buildingAssets = [Bank, Blacksmith, Fountain, Grass, Mine, Restaurant, JunctionRoad, StraightRoad, BuildingCorner, BuildingCornerTrap, BuildingStraight, BuildingStraightTrap]
const blockAssets = [EmptyBlock, Rock, WoodenFrame, CopperOre, IronOre, GoldOre, SilverOre, UnobtainiumOre]
const floorAssets = [Floor, FloorCorner, FloorWall, FloorDoorLeft, FloorDoorRight, FloorStairs]
const enemyAssets = [Dragon, Skeleton, Zombie]
const itemAssets = [WoodenPickaxe, WoodenSword, Copper, Gold, Iron, RockItem, Silver, Unobtanium, Wood]
const burgerAssets = [Bacon, BunDown, BunUp, Meat, Salad, Sauce, Tomato, Cheese]

const assets = [Player, TableLeft, TableRight, ...buildingAssets, ...blockAssets, ...floorAssets, ...enemyAssets, ...itemAssets, ...burgerAssets]

const AssetImporter = () => {
    return (
        <defs>
            {assets.map((AssetComponent, index) => (
                <AssetComponent key={index} />
            ))}
        </defs>
    )
}

export default AssetImporter