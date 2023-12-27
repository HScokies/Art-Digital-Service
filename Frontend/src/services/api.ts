import { IRow, orderBy, param } from "src/components/dataGridView/interfaces"
import UsersMock from './mock/usersMock.json'

export class API{
    static protocol: "http" | "https" = "http"
    static baseURL = "example.com"
    private static readonly URL = new URL(`${this.protocol}://${this.baseURL}/`)

    static getUsers = (filters: param[], search?: string, orderBy?: orderBy): IRow[] => {
        const getUsersURL = new URL(API.URL);
        if (search){
            getUsersURL.searchParams.append("search", search)
        }
        if (orderBy){
            getUsersURL.searchParams.append("orderBy",orderBy.column)
            getUsersURL.searchParams.append("order",orderBy.order)
        }
        filters.forEach((f) => {
            getUsersURL.searchParams.append(f.name, String(f.value))
        })
        console.debug(getUsersURL)
        return (UsersMock as unknown) as IRow[]        
    }

    static exportUsers = (ids?: Set<number>) => {
        const exportUsersURL = new URL(API.URL);
        ids?.forEach((id) => {
            exportUsersURL.searchParams.append("id", String(id))
        })
        console.debug(exportUsersURL)
    }

    static deleteUsers = (ids: Set<number>) => {
        const deleteUserURL = new URL(API.URL);
        ids?.forEach((id) => {
            deleteUserURL.searchParams.append("id", String(id))
        })
        console.debug(deleteUserURL)
    }
}