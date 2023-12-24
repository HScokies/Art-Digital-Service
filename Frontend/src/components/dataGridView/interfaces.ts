export interface orderBy{
    column: string,
    order: 'asc' | 'desc'
}

/**
GET Parameters to exclude from query
**/
export interface param{    
    id: string,
    title: string,
    name: string,
    value: string | boolean | number,
    isActive: boolean,
}

export interface IColumnOptions{
    column: string,
    options: param[]
}

export interface IHeaderCell {
    id: string,
    title: string,
    options?: param[],
    setSort: React.Dispatch<React.SetStateAction<orderBy | undefined>>,
    activeSort?: orderBy,
    setFilters?: React.Dispatch<React.SetStateAction<IColumnOptions[]>>,
    activeFilters?: IColumnOptions[]
}

export interface IColumn{
    id: string,
    title: string,
    options?: param[]
}