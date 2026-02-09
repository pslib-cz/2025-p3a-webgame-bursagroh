import React from 'react'
import styles from './loadingWrapper.module.css'
import type { TLoadingWrapperContextState } from '../../types/context'
import Text from '../Text'

type LoadingWrapperProps<T> = {
    context: React.Context<T>
} & React.PropsWithChildren

const LoadingWrapper = <T extends TLoadingWrapperContextState>({ children, context }: LoadingWrapperProps<T>) => {
    const ctx = React.useContext(context)

    if (!ctx) {
        return children
    }

    if (ctx.isPending) {
        return <Text size="h2" className={styles.loading}>Loading...</Text>
    }

    if (ctx.isError) {
        return <Text size="h2" className={styles.loading}>Error loading data!</Text>
    }

    if (ctx.isSuccess) {
        return children
    }
}

export default LoadingWrapper