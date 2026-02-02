import { useMutation } from "@tanstack/react-query"
import { updatePlayerScreenMutation } from "../../api/player"
import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { moveBankMoneyMutation } from "../../api/bank"
import { useNavigate } from "react-router"
import Input from "../../components/Input"
import MoneyIcon from "../../assets/icons/MoneyIcon"
import { PlayerContext } from "../../providers/game/PlayerProvider"
import { InventoryContext } from "../../providers/game/InventoryProvider"
import { BankContext } from "../../providers/game/BankProvider"
import styles from "./bank.module.css"
import CloseIcon from "../../assets/icons/CloseIcon"
import BankInventoryItem from "../../components/item/BankInventoryItem"
import { countInventoryItems, removeEquippedItemFromInventory } from "../../utils/inventory"
import BankItem from "../../components/item/BankItem"
import useBlur from "../../hooks/useBlur"

// const InventoryItem = ({ playerId, item }: { playerId: string, item: InventoryItemType }) => {
//     const { mutateAsync: moveBankItemAsync } = useMutation(moveBankItemMutation(playerId, item.inventoryItemId))

//     const handleClick = () => {
//         moveBankItemAsync()
//     }

//     return (
//         <div>
//             Item: {item.itemInstance.item.name}
//             <button onClick={handleClick}>move</button>
//         </div>
//     )
// }

// const BankItem = ({ playerId, item }: { playerId: string, item: InventoryItemType }) => {
//     const { mutateAsync: moveBankItemAsync } = useMutation(moveBankItemMutation(playerId, item.inventoryItemId))

//     const handleClick = () => {
//         moveBankItemAsync()
//     }

//     return (
//         <div>
//             Item: {item.itemInstance.item.name}
//             <button onClick={handleClick}>move</button>
//         </div>
//     )
// }

const BankScreen = () => {
    useBlur(true)
    
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const bank = React.useContext(BankContext)!.bank!

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))
    const { mutateAsync: moveBankMoneyAsync } = useMutation(moveBankMoneyMutation(playerId))

    const [toBankAmount, setToBankAmount] = React.useState(0)
    const [toPlayerAmount, setToPlayerAmount] = React.useState(0)

    const handleClick = async () => {
        await updatePlayerScreenAsync()

        navigate("/game/city")
    }

    const handleTransferToBank = async () => {
        await moveBankMoneyAsync({ amount: toBankAmount ?? 0, direction: "ToBank" })
        setToBankAmount(0)
    }

    const handleTransferToPlayer = async () => {
        await moveBankMoneyAsync({ amount: toPlayerAmount ?? 0, direction: "ToPlayer" })
        setToPlayerAmount(0)
    }

    const updatedInventory = removeEquippedItemFromInventory([...inventory], player.activeInventoryItemId)
    const inventoryItems = countInventoryItems(updatedInventory)

    const updatedBank = removeEquippedItemFromInventory([...bank], player.activeInventoryItemId)
    const bankItems = countInventoryItems(updatedBank)

    return (
        <div className={styles.container}>
            <div className={styles.bankContainer}>
                <span className={styles.heading}>Player</span>
                <span className={styles.heading}>Bank</span>
                <div className={styles.transferContainer}>
                    <Input type="number" placeholder="Amount" value={toBankAmount} onChange={(e) => setToBankAmount(Number.parseInt(e.target.value))} />
                    <div className={styles.transferSubContainer}>
                        <span className={styles.balance}>/ {player.money}</span>
                        <MoneyIcon className={styles.money} width={32} height={32} />
                    </div>
                    <button onClick={handleTransferToBank}>Send</button>
                </div>
                <div className={styles.transferContainer}>
                    <button onClick={handleTransferToPlayer}>Send</button>
                    <Input type="number" placeholder="Amount" value={toPlayerAmount} onChange={(e) => setToPlayerAmount(Number.parseInt(e.target.value))} />
                    <div className={styles.transferSubContainer}>
                        <span className={styles.balance}>/ {player.bankBalance}</span>
                        <MoneyIcon className={styles.money} width={32} height={32} />
                    </div>
                </div>
                <div className={styles.itemContainer}>
                    {Object.entries(inventoryItems).map(([itemId, count]) => (
                        <BankInventoryItem key={itemId} item={updatedInventory.find(item => item.itemInstance.item.itemId === Number(itemId))!} count={count} />
                    ))}
                </div>
                <div className={styles.itemContainer}>
                    {Object.entries(bankItems).map(([itemId, count]) => (
                        <BankItem key={itemId} item={updatedBank.find(item => item.itemInstance.item.itemId === Number(itemId))!} count={count} />
                    ))}
                </div>
                <CloseIcon width={24} height={24} className={styles.close} onClick={handleClick} />
            </div>
        </div>
    )
}

export default BankScreen
