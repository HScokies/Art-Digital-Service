import { IColumnOptions, orderBy } from "src/components/dataGridView/interfaces"

export class API{
    static protocol: "http" | "https" = "http"
    static baseURL = "example.com"
    private static readonly URL = new URL(`${this.protocol}://${this.baseURL}/`)

    static getUsers = (filters: IColumnOptions[], orderBy?: orderBy) => {
        const getUsersURL = new URL(API.URL);
        if (orderBy){
            getUsersURL.searchParams.append("orderBy",orderBy.column)
            getUsersURL.searchParams.append("order",orderBy.order)
        }
        for (let i=0; i < filters.length; i++){
            const column = filters[i]
            for(let j=0; j < column.options.length; j++){
                const filter = column.options[j]
                if (!filter.isActive){//if param is not active => exclude it from search
                    getUsersURL.searchParams.append(filter.name, String(filter.value))
                }
            }
        }
        console.debug("base URL:", API.URL, "Search", getUsersURL)
    }
}