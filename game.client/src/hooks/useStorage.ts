import React from 'react'

const useStorage = <T,>(key: string, initialValue: T) => {
	const [value, setValue] = React.useState<T>(() => {
		const stored = localStorage.getItem(key)
		if (stored === null) return initialValue
		try {
			return JSON.parse(stored) as T
		} catch {
			return initialValue
		}
	})

	React.useEffect(() => {
		localStorage.setItem(key, JSON.stringify(value))
	}, [key, value])

	const deleteValue = React.useCallback(() => {
		localStorage.removeItem(key)
		setValue(initialValue)
	}, [key, initialValue])

	return [value, setValue, deleteValue] as const
}

export default useStorage