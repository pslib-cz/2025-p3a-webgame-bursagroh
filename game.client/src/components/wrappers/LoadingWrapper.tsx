import React from 'react'

export type TLoadingWrapperContextState = {
    isPending: boolean
    isError: boolean
    isSuccess: boolean
} | null

type LoadingWrapperProps<T> = {
    context: React.Context<T>
} & React.PropsWithChildren

const LoadingWrapper = <T extends TLoadingWrapperContextState>({ children, context }: LoadingWrapperProps<T>) => {
    const ctx = React.useContext(context)

    if (!ctx) {
        return children
    }

    if (ctx.isPending) {
        return <div>Loading...</div>
    }

    if (ctx.isError) {
        return <div>Error loading data</div>
    }

    if (ctx.isSuccess) {
        return children
    }
}

export default LoadingWrapper