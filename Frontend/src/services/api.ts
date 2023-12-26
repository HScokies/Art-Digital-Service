import { IRow, orderBy, param } from "src/components/dataGridView/interfaces"
import UsersMock from './mock/usersMock.json'

export class API{
    static protocol: "http" | "https" = "http"
    static baseURL = "example.com"
    private static readonly URL = new URL(`${this.protocol}://${this.baseURL}/`)

    static getUsers = (filters: param[], orderBy?: orderBy): IRow[] => {
        const getUsersURL = new URL(API.URL);
        if (orderBy){
            getUsersURL.searchParams.append("orderBy",orderBy.column)
            getUsersURL.searchParams.append("order",orderBy.order)
        }
        filters.forEach((f) => {
            getUsersURL.searchParams.append(f.name, String(f.value))
        })
        console.debug("base URL:", API.URL, "Search", getUsersURL)
        return (UsersMock as unknown) as IRow[]
    }
}