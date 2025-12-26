export type ModuleConfig = {
    views: {
        app: AppView[]
    }
}

export type AppView = {
    route: string
    component: React.ComponentType<any> // or more specific if you want
    auth?: boolean
}
