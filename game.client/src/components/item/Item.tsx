import React from 'react'
import styles from './item.module.css'
import Tooltip from '../Tooltip'
import Asset from '../SVG/Asset'
import ConditionalDisplay from '../wrappers/ConditionalDisplay'
import WeightIcon from '../../icons/WeightIcon'
import ArrowLeftIcon from '../../icons/ArrowLeftIcon'
import ArrowLeftDoubleIcon from '../../icons/ArrowLeftDoubleIcon'
import type { AssetType } from '../../types/asset'
import ArrowRightIcon from '../../icons/ArrowRightIcon'
import ArrowRightDoubleIcon from '../../icons/ArrowRightDoubleIcon'
import Text from '../Text'

type ItemProps = {
    tooltipHeading: string
    tooltipText: string
    assetType: AssetType | undefined
    durability?: number
    weight?: number
    isWeightDown?: boolean
    amount?: number
    price?: number
    onSingleMoveLeft?: () => void
    onMultipleMoveLeft?: () => void
    onSingleMoveRight?: () => void
    onMultipleMoveRight?: () => void
    draggable?: boolean
    onDragStart?: (event: React.DragEvent<HTMLDivElement>) => void
    onClick?: () => void
}

const Item: React.FC<ItemProps> = ({ tooltipHeading, tooltipText, assetType, durability, weight, isWeightDown, amount, price, onSingleMoveLeft, onMultipleMoveLeft, onSingleMoveRight, onMultipleMoveRight, draggable, onDragStart, onClick }) => {
    return (
        <Tooltip heading={tooltipHeading} text={tooltipText}>
            <div className={`${styles.container} ${onClick ? styles.clickable : ''}`} draggable={draggable} onDragStart={onDragStart} onClick={onClick}>
                <svg width="128" height="128" viewBox="0 0 128 128">
                    <Asset assetType={assetType} width={128} height={128} />
                </svg>
                <ConditionalDisplay condition={durability !== undefined && durability !== 0}>
                    <Text size="h3" className={styles.durability}>{durability}</Text>
                </ConditionalDisplay>
                <ConditionalDisplay condition={weight !== undefined}>
                    <div className={isWeightDown ? styles.weightDown : styles.weight}>
                        <WeightIcon className={styles.weightIcon} width={24} height={24} />
                        <Text size="h3">{weight}</Text>
                    </div>
                </ConditionalDisplay>
                <ConditionalDisplay condition={amount !== undefined}>
                    <Text size="h3" className={styles.amount}>{amount}x</Text>
                </ConditionalDisplay>
                <ConditionalDisplay condition={price !== undefined}>
                    <Text size="h3" className={styles.price}>{price}$</Text>
                </ConditionalDisplay>
                <ConditionalDisplay condition={onSingleMoveLeft !== undefined}>
                    <ArrowLeftIcon className={styles.transferSingleLeft} width={32} height={32} onClick={onSingleMoveLeft} />
                </ConditionalDisplay>
                <ConditionalDisplay condition={onMultipleMoveLeft !== undefined}>
                    <ArrowLeftDoubleIcon className={styles.transferMultiLeft} width={32} height={32} onClick={onMultipleMoveLeft} />
                </ConditionalDisplay>
                <ConditionalDisplay condition={onSingleMoveRight !== undefined}>
                <ArrowRightIcon className={styles.transferSingleRight} width={32} height={32} onClick={onSingleMoveRight} />
                </ConditionalDisplay>
                <ConditionalDisplay condition={onMultipleMoveRight !== undefined}>
                <ArrowRightDoubleIcon className={styles.transferMultiRight} width={32} height={32} onClick={onMultipleMoveRight} />
                </ConditionalDisplay>
            </div>
        </Tooltip>
    )
}

export default Item