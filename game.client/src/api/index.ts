import { QueryClient } from "@tanstack/react-query"
import type { API, NoContentURL, Param, Query } from "../types/api"

const SERVER_URL = import.meta.env.VITE_SERVER_URL as string

export const queryClient = new QueryClient()

const formatURL = (url: string, params: Param, query: Query) => {
    let formattedURL = SERVER_URL + url

    for (const [key, value] of Object.entries(params)) {
        formattedURL = formattedURL.replace(`{${key}}`, encodeURIComponent(value))
    }

    const onlyStringQuery: Record<string, string> = {}
    for (const [key, value] of Object.entries(query)) {
        onlyStringQuery[key] = value.toString()
    }

    const queryString = new URLSearchParams(onlyStringQuery).toString()
    if (queryString) {
        formattedURL += `?${queryString}`
    }

    return formattedURL
}

export const api = {
    get: async <URL extends keyof API["get"], Params extends API["get"][URL]["params"], Query extends API["get"][URL]["query"]>(url: URL, params: Params, query: Query) => {
        const uri = formatURL(url, params, query)
        const response = await fetch(uri)

        if (!response.ok) {
            throw new Error(`Error getting ${uri}: ${response.statusText}`)
        }

        return await response.json() as API["get"][URL]["res"][200]
    },
    getWith204: async <URL extends keyof Pick<API["get"], NoContentURL>, Params extends API["get"][URL]["params"], Query extends API["get"][URL]["query"]>(url: URL, params: Params, query: Query) => {
        const uri = formatURL(url, params, query)
        const response = await fetch(uri)

        if (!response.ok) {
            throw new Error(`Error getting ${uri}: ${response.statusText}`)
        }

        if (response.status === 204) {
            return [] as API["get"][URL]["res"][204]
        }

        return await response.json() as API["get"][URL]["res"][200]
    },
    post: async <URL extends keyof API["post"], Params extends API["post"][URL]["params"], Query extends API["post"][URL]["query"], Body extends API["post"][URL]["body"]>(url: URL, params: Params, query: Query, body: Body) => {
        const uri = formatURL(url, params, query)
        const response = await fetch(uri, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(body),
        })

        if (!response.ok) {
            throw new Error(`Error posting to ${uri}: ${response.statusText}`)
        }

        return await response.json() as API["post"][URL]["res"][200]
    },
    put: async <URL extends keyof API["put"], Params extends API["put"][URL]["params"], Query extends API["put"][URL]["query"], Body extends API["put"][URL]["body"]>(url: URL, params: Params, query: Query, body: Body) => {
        const uri = formatURL(url, params, query)
        const response = await fetch(uri, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(body),
        })

        if (!response.ok) {
            throw new Error(`Error putting to ${uri}: ${response.statusText}`)
        }

        return await response.json() as API["put"][URL]["res"][200]
    },
    patch: async <URL extends keyof API["patch"], Params extends API["patch"][URL]["params"], Query extends API["patch"][URL]["query"], Body extends API["patch"][URL]["body"]>(url: URL, params: Params, query: Query, body: Body) => {
        const uri = formatURL(url, params, query)
        const response = await fetch(uri, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(body),
        })

        if (!response.ok) {
            throw new Error(`Error patching to ${uri}: ${response.statusText}`)
        }

        const text = await response.text()

        if (!text) {
            return {} as API["patch"][URL]["res"][200]
        }

        return await JSON.parse(text) as API["patch"][URL]["res"][200]
    },
    patchWith204: async <URL extends keyof Pick<API["patch"], "/api/Recipe/{recipeId}/Action/start">, Params extends API["patch"][URL]["params"], Query extends API["patch"][URL]["query"], Body extends API["patch"][URL]["body"]>(url: URL, params: Params, query: Query, body: Body) => {
        const uri = formatURL(url, params, query)
        const response = await fetch(uri, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(body),
        })

        if (!response.ok) {
            throw new Error(`Error patching to ${uri}: ${response.statusText}`)
        }

        if (response.status === 204) {
            return [] as API["patch"][URL]["res"][204]
        }

        return await response.json() as API["patch"][URL]["res"][200]
    },
    delete: async <URL extends keyof API["delete"], Params extends API["delete"][URL]["params"], Query extends API["delete"][URL]["query"]>(url: URL, params: Params, query: Query) => {
        const uri = formatURL(url, params, query)
        const response = await fetch(uri, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
            }
        })

        if (!response.ok) {
            throw new Error(`Error deleting to ${uri}: ${response.statusText}`)
        }

        return await response.json() as API["delete"][URL]["res"][200]
    },
}