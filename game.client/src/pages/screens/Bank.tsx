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
import { groupInventoryItems } from "../../utils/inventory"
import BankItem from "../../components/item/BankItem"
import useBlur from "../../hooks/useBlur"
import SendIcon from "../../assets/icons/SendIcon"
import useNotification from "../../hooks/useNotification"
import useKeyboard from "../../hooks/useKeyboard"

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
    const {genericError} = useNotification()

    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const bank = React.useContext(BankContext)!.bank!

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City", genericError))
    const { mutateAsync: moveBankMoneyAsync } = useMutation(moveBankMoneyMutation(playerId, genericError))

    const [toBankAmount, setToBankAmount] = React.useState(0)
    const [toPlayerAmount, setToPlayerAmount] = React.useState(0)

    const handleEscape = async () => {
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

    const inventoryItems = groupInventoryItems(inventory)
    const bankItems = groupInventoryItems(bank)

    useKeyboard("Escape", handleEscape)

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
                    <SendIcon className={styles.sendIcon} width={32} height={32} onClick={handleTransferToBank} />
                </div>
                <div className={styles.transferContainer}>
                    <SendIcon className={styles.sendIconFlipped} width={32} height={32} onClick={handleTransferToPlayer} />
                    <Input type="number" placeholder="Amount" value={toPlayerAmount} onChange={(e) => setToPlayerAmount(Number.parseInt(e.target.value))} />
                    <div className={styles.transferSubContainer}>
                        <span className={styles.balance}>/ {player.bankBalance}</span>
                        <MoneyIcon className={styles.money} width={32} height={32} />
                    </div>
                </div>
                <div className={styles.itemContainer}>
                    {Object.entries(inventoryItems).map(([itemString, inventoryItems]) => (
                        <BankInventoryItem key={itemString} items={inventory.filter(item => inventoryItems.includes(item.inventoryItemId))!} />
                    ))}
                </div>
                <div className={styles.itemContainer}>
                    {Object.entries(bankItems).map(([itemString, inventoryItems]) => (
                        <BankItem key={itemString} items={bank.filter(item => inventoryItems.includes(item.inventoryItemId))!} />
                    ))}
                </div>
                <CloseIcon width={24} height={24} className={styles.close} onClick={handleEscape} />
            </div>
        </div>
    )
}

export default BankScreen
