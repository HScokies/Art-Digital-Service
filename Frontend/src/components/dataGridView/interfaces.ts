export interface orderBy{
    column: string,
    asc: boolean
}

/**
GET Parameters to exclude from query
**/
export interface param{    
    title: string,
    name: string,
    value: string | boolean | number,
}

export interface filter{
    column: string,
    param: param
}

export interface IHeaderCell {
    index: number,
    sizePx?: number,
    columnData: IColumn
    setSort: React.Dispatch<React.SetStateAction<orderBy | undefined>>,
    activeSort?: orderBy,
    setActiveFilters: React.Dispatch<React.SetStateAction<Set<filter>>>,
    activeFilters: Set<filter>
}

export interface IColumn{
    id: string,
    title: string,
    sizePx?: number
    filters?: param[]
}

export interface IData{
    currentPage: number,
    pagesTotal: number
    rows: IRow[]
}

export interface IRow{
    id: number
}
