export interface orderBy{
    column: string,
    order: 'asc' | 'desc'
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
    columnData: IColumn
    setSort: React.Dispatch<React.SetStateAction<orderBy | undefined>>,
    activeSort?: orderBy,
    setActiveFilters: React.Dispatch<React.SetStateAction<Set<filter>>>,
    activeFilters: Set<filter>
}

export interface IColumn{
    id: string,
    title: string,
    filters?: param[]
}

export interface IRow{
    id: number,
    data: string[]
}

export interface pagination{
    rows: number,
    page: number
}
