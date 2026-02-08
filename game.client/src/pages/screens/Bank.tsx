import { useMutation } from "@tanstack/react-query"
import React from "react"
import { PlayerIdContext } from "../../providers/global/PlayerIdProvider"
import { moveBankMoneyMutation } from "../../api/bank"
import Input from "../../components/Input"
import { PlayerContext } from "../../providers/global/PlayerProvider"
import { InventoryContext } from "../../providers/game/InventoryProvider"
import BankProvider, { BankContext } from "../../providers/game/BankProvider"
import styles from "./bank.module.css"
import CloseIcon from "../../assets/icons/CloseIcon"
import BankInventoryItem from "../../components/item/BankInventoryItem"
import { groupInventoryItems } from "../../utils/inventory"
import BankItem from "../../components/item/BankItem"
import useBlur from "../../hooks/useBlur"
import SendIcon from "../../assets/icons/SendIcon"
import useNotification from "../../hooks/useNotification"
import useKeyboard from "../../hooks/useKeyboard"
import ProviderGroupLoadingWrapper from "../../components/wrappers/ProviderGroupLoadingWrapper"
import type { TLoadingWrapperContextState } from "../../components/wrappers/LoadingWrapper"
import ArrayDisplay from "../../components/wrappers/ArrayDisplay"
import useLock from "../../hooks/useLock"
import useLink from "../../hooks/useLink"

const BankScreenWithContext = () => {
    useBlur(true)
    
    const moveToPage = useLink()
    const {genericError} = useNotification()
    const handleLock = useLock()

    const playerId = React.useContext(PlayerIdContext)!.playerId!
    const player = React.useContext(PlayerContext)!.player!
    const inventory = React.useContext(InventoryContext)!.inventory!
    const bank = React.useContext(BankContext)!.bank!

    const { mutateAsync: moveBankMoneyAsync } = useMutation(moveBankMoneyMutation(playerId, genericError))

    const [toBankAmount, setToBankAmount] = React.useState(0)
    const [toPlayerAmount, setToPlayerAmount] = React.useState(0)

    const handleEscape = async () => {
        await moveToPage("city", true)
    }

    const handleTransferToBank = async () => {
        await handleLock(async () => {
            await moveBankMoneyAsync({ amount: toBankAmount ?? 0, direction: "ToBank" })
            setToBankAmount(0)
        })
    }

    const handleTransferToPlayer = async () => {
        await handleLock(async () => {
            await moveBankMoneyAsync({ amount: toPlayerAmount ?? 0, direction: "ToPlayer" })
            setToPlayerAmount(0)
        })
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
                        <span className={styles.balance}>/ {player.money} $</span>
                    </div>
                    <SendIcon className={styles.sendIcon} width={32} height={32} onClick={handleTransferToBank} />
                </div>
                <div className={styles.transferContainer}>
                    <SendIcon className={styles.sendIconFlipped} width={32} height={32} onClick={handleTransferToPlayer} />
                    <Input type="number" placeholder="Amount" value={toPlayerAmount} onChange={(e) => setToPlayerAmount(Number.parseInt(e.target.value))} />
                    <div className={styles.transferSubContainer}>
                        <span className={styles.balance}>/ {player.bankBalance} $</span>
                    </div>
                </div>
                <div className={styles.itemContainer}>
                    <ArrayDisplay elements={Object.entries(inventoryItems).map(([itemString, inventoryItems]) => (
                        <BankInventoryItem key={itemString} items={inventory.filter(item => inventoryItems.includes(item.inventoryItemId))!} />
                    ))} ifEmpty={<span className={styles.text}>Empty inventory</span>} />
                </div>
                <div className={styles.itemContainer}>
                    <ArrayDisplay elements={Object.entries(bankItems).map(([itemString, inventoryItems]) => (
                        <BankItem key={itemString} items={bank.filter(item => inventoryItems.includes(item.inventoryItemId))!} />
                    ))} ifEmpty={<span className={styles.text}>Empty bank</span>} />
                </div>
                <CloseIcon width={24} height={24} className={styles.close} onClick={handleEscape} />
            </div>
        </div>
    )
}

const BankScreen = () => {
    return (
        <ProviderGroupLoadingWrapper providers={[BankProvider]} contextsToLoad={[BankContext] as Array<React.Context<TLoadingWrapperContextState>>}>
            <BankScreenWithContext />
        </ProviderGroupLoadingWrapper>
    )
}

export default BankScreen
