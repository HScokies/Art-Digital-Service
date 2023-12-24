import './style.scss'
import HeaderCell from './HeaderCell'
import { useEffect, useState } from 'react'
import { IColumn, IColumnOptions, orderBy } from './interfaces'
import { API } from 'src/services'

interface props {
    columns: IColumn[],
}



const DataGridView = ({ columns }: props) => {
    const [activeSort, setActiveSort] = useState<orderBy | undefined>()
    const [filters, setFilters] = useState<IColumnOptions[]>([])

    useEffect(() => {
        let defaultFiltersState: IColumnOptions[] = [];
        columns.forEach((c) => {
            if (c.options){
              const newFilter = {column: c.id, options: c.options}
              defaultFiltersState.push(newFilter)
            }
        })   
        setFilters(defaultFiltersState)
        },[])

    useEffect(() => {
        API.getUsers(filters, activeSort)
        console.debug("Order by:", activeSort)
        console.debug("Active filters:", filters)
    }, [activeSort, filters])    

    return (
        <table className='datagrid'>
            <thead className='datagrid_header'>
                <tr className='datagrid-row'>
                    {
                        columns.map((e) => (
                            <HeaderCell key={e.id} id={e.id} title={e.title} setSort={setActiveSort} activeSort={activeSort} options={e.options} setFilters={setFilters} activeFilters={filters} />
                        ))
                    }
                </tr>
            </thead>
        </table>
    )
}

export default DataGridView