import { useMutation, useQuery } from "@tanstack/react-query"
import { getPlayerInventoryQuery, getPlayerQuery, updatePlayerScreenMutation } from "../../api/player"
import React from "react"
import { PlayerIdContext } from "../../providers/PlayerIdProvider"
import { getBankInventoryQuery, moveBankItemMutation, moveBankMoneyMutation } from "../../api/bank"
import type { InventoryItem as InventoryItemType } from "../../types/api/models/player"
import { useNavigate } from "react-router"

const InventoryItem = ({ playerId, item }: {playerId: string, item: InventoryItemType}) => {
    const { mutateAsync: moveBankItemAsync } = useMutation(moveBankItemMutation(playerId, item.inventoryItemId))

    const handleClick = () => {
        moveBankItemAsync()
    }

    return (
        <div>
            Item: {item.itemInstance.item.name}
            <button onClick={handleClick}>move</button>
        </div>
    )
}

const BankItem = ({ playerId, item }: {playerId: string, item: InventoryItemType}) => {
    const { mutateAsync: moveBankItemAsync } = useMutation(moveBankItemMutation(playerId, item.inventoryItemId))

    const handleClick = () => {
        moveBankItemAsync()
    }

    return (
        <div>
            Item: {item.itemInstance.item.name}
            <button onClick={handleClick}>move</button>
        </div>
    )
}

const BankScreen = () => {
    const navigate = useNavigate()
    const playerId = React.useContext(PlayerIdContext)!.playerId!

    const player = useQuery(getPlayerQuery(playerId))
    const bank = useQuery(getBankInventoryQuery(playerId))
    const inventory = useQuery(getPlayerInventoryQuery(playerId))

    const { mutateAsync: updatePlayerScreenAsync } = useMutation(updatePlayerScreenMutation(playerId, "City"))
    const {mutateAsync: moveBankMoneyAsync} = useMutation(moveBankMoneyMutation(playerId))

    const [amount, setAmount] = React.useState(0)

    const handleClick = async () => {
        await updatePlayerScreenAsync()
        
        navigate("/game/city")
    }

    const handleTransferToBank = () => {
        moveBankMoneyAsync({amount, direction: "ToBank"})
    }

    const handleTransferToPlayer = () => {
        moveBankMoneyAsync({amount, direction: "ToPlayer"})
    }

    if (player.isError || bank.isError || inventory.isError) {
        return <div>Error loading.</div>
    }

    if (player.isPending || bank.isPending || inventory.isPending) {
        return <div>Loading...</div>
    }

    if (player.isSuccess && bank.isSuccess && inventory.isSuccess) {
        return (
            <>
                <div>Bank</div>
                <p>Player Money: {player.data.money}</p>
                <p>Player Bank Balance: {player.data.bankBalance}</p>
                <input type="number" value={amount} onChange={(e) => setAmount(Number(e.target.value))} />
                <button onClick={handleTransferToBank}>To Bank</button>
                <button onClick={handleTransferToPlayer}>To Player</button>
                {bank.data.map((item) => (
                    <BankItem key={item.inventoryItemId} playerId={playerId} item={item} />
                ))}
                <div>Inventory</div>
                {inventory.data.map((item) => (
                    <InventoryItem key={item.inventoryItemId} playerId={playerId} item={item} />
                ))}
                <button onClick={handleClick}>close</button>
            </>
        )
    }
}

export default BankScreen
