import React from 'react'
import styles from './loadingWrapper.module.css'
import type { TLoadingWrapperContextState } from '../../types/context'

type LoadingWrapperProps<T> = {
    context: React.Context<T>
} & React.PropsWithChildren

const LoadingWrapper = <T extends TLoadingWrapperContextState>({ children, context }: LoadingWrapperProps<T>) => {
    const ctx = React.useContext(context)

    if (!ctx) {
        return children
    }

    if (ctx.isPending) {
        return <span className={styles.loading}>Loading...</span>
    }

    if (ctx.isError) {
        return <span className={styles.loading}>Error loading data!</span>
    }

    if (ctx.isSuccess) {
        return children
    }
}

export default LoadingWrapper