import React from 'react'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import useNotification from '../../hooks/useNotification'
import { rentPickMutation } from '../../api/mine'
import useLock from '../../hooks/useLock'
import Item from './Item'

const RentItem: React.FC = () => {
    const {genericError} = useNotification()
    const handleLock = useLock()
    
    const playerId = React.useContext(PlayerIdContext)!.playerId!
    
    const { mutateAsync: rentPickAsync } = useMutation(rentPickMutation(playerId, 1, genericError))

    const handleClick = async () => {
        await handleLock(async () => {
            await rentPickAsync()
        })
    }

    return (
        <Item tooltipHeading="Rent a PICK!"
            tooltipText="Rent a pickaxe for 5$. It will be returned automatically after you leave."
            assetType="rented_pickaxe"
            price={5}
            onClick={handleClick} />
    )
}

export default RentItem