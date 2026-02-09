import type { ScreenType } from "../types/api/models/player";
import type { PageType } from "../types/page";

export const pageTypeToURL = (pageType: PageType, saveString?: string) => {
    switch (pageType) {
        case "bank":
            return "/game/bank"
        case "blacksmith":
            return "/game/blacksmith"
        case "city":
            return "/game/city"
        case "fight":
            return "/game/fight"
        case "floor":
            return "/game/floor"
        case "fountain":
            return "/game/fountain"
        case "lose":
            return "/game/lose"
        case "mine":
            return "/game/mine"
        case "restaurant":
            return "/game/restaurant"
        case "win":
            return "/game/win"
        case "load":
            return "/load"
        case "save":
            return "/save"
        case "settings":
            return "/settings"
        case "root":
            return "/"
        case "loadSave":
            return `/load/${encodeURIComponent(saveString!)}`
    }
}

export const screenTypeToPageType = (screenType: ScreenType): PageType => {
    switch (screenType) {
        case "City":
            return "city"
        case "Bank":
            return "bank"
        case "Mine":
            return "mine"
        case "Restaurant":
            return "restaurant"
        case "Blacksmith":
            return "blacksmith"
        case "Floor":
            return "floor"
        case "Fight":
            return "fight"
        case "Fountain":
            return "fountain"
        case "Win":
            return "win"
        case "Lose":
            return "lose"
    }
}

export const pageTypeToScreenType = (pageType: Omit<PageType, "load" | "save" | "settings" | "root" | "loadSave">): ScreenType => {
    switch (pageType) {
        case "city":
            return "City"
        case "bank":
            return "Bank"
        case "mine":
            return "Mine"
        case "restaurant":
            return "Restaurant"
        case "blacksmith":
            return "Blacksmith"
        case "floor":
            return "Floor"
        case "fight":
            return "Fight"
        case "fountain":
            return "Fountain"
        case "win":
            return "Win"
        case "lose":
            return "Lose"
    }

    return undefined as never
}

export const screenTypeToURL = (screenType: ScreenType) => {
    switch (screenType) {
        case "City":
            return "/game/city"
        case "Bank":
            return "/game/bank"
        case "Mine":
            return "/game/mine"
        case "Restaurant":
            return "/game/restaurant"
        case "Blacksmith":
            return "/game/blacksmith"
        case "Floor":
            return `/game/floor`
        case "Fight":
            return "/game/fight"
        case "Fountain":
            return "/game/fountain"
        case "Win":
            return "/game/win"
        case "Lose":
            return "/game/lose"
    }
}

