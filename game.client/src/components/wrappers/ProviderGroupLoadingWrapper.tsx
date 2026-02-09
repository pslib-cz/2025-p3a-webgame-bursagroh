import React from 'react'
import type { TLoadingWrapperContextState } from '../../types/context'
import LoadingWrapper from './LoadingWrapper'

type ProviderGroupLoadingWrapperProps = {
    providers: Array<React.FC<React.PropsWithChildren>>
    contextsToLoad: Array<React.Context<TLoadingWrapperContextState>>
}

const ProviderGroupLoadingWrapper: React.FC<React.PropsWithChildren<ProviderGroupLoadingWrapperProps>> = ({ children, providers, contextsToLoad }) => {
    return (
        <>
            {providers.reduceRight(
                (acc, Provider) => (
                    <Provider>{acc}</Provider>
                ),
                contextsToLoad.reduceRight(
                    (acc, context) => (
                        <LoadingWrapper context={context}>{acc}</LoadingWrapper>
                    ),
                    children
                )
            )}
        </>
    )
}

export default ProviderGroupLoadingWrapper