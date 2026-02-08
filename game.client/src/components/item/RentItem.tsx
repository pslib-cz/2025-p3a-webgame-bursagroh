import React from 'react'
import Asset from '../SVG/Asset'
import { PlayerIdContext } from '../../providers/global/PlayerIdProvider'
import { useMutation } from '@tanstack/react-query'
import styles from './rentItem.module.css'
import Tooltip from '../Tooltip'
import useNotification from '../../hooks/useNotification'
import { rentPickMutation } from '../../api/mine'
import useLock from '../../hooks/useLock'

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
        <Tooltip heading="Rent a PICK!" text="Rent a pickaxe for 5$. It will be returned automatically after you leave.">
            <div className={styles.container} onClick={handleClick}>
                <svg width="128" height="128" viewBox="0 0 128 128">
                    <Asset assetType="rented_pickaxe" width={128} height={128} />
                </svg>
                <span className={styles.price}>5$</span>
            </div>
        </Tooltip>
    )
}

export default RentItem