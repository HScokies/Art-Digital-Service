import './style.scss'
import HeaderCell from './HeaderCell'
import { useEffect, useState } from 'react'
import { IColumn, IRow, filter, orderBy, param } from './interfaces'
import { API } from 'src/services'
import DatagridRow from './Row'

interface props {
    columns: IColumn[],
    source: (filters: Set<param>, orderBy: orderBy) => Object[]
}



const DataGridView = ({ columns, source }: props) => {
    const [data, setData] = useState<IRow[]>([])
    
    //array of highlighted rows ids
    const [highlightedRows, setHighlightedRows] = useState(new Set<Number>([]))
    const [activeSort, setActiveSort] = useState<orderBy | undefined>()
    const [activeFilters, setActiveFilters] = useState(new Set<filter>([]))

    //data handler
    useEffect(() => {
        let params: param[] = []
        activeFilters.forEach((f) => params.push(f.param))

        const data = API.getUsers(params, activeSort)
        setData(data)
    }, [activeSort, activeFilters])

    //highlight handler
    useEffect(() => {
        console.debug("highlighted:", highlightedRows)
    }, [highlightedRows])



    return (
        <table className='datagrid'>
            <thead className='datagrid_header'>
                <tr className='datagrid-row'>
                    <th className='row-select-header'></th>
                    {                        
                        columns.map((e, i) => (
                            <HeaderCell key={e.id} index={i}  columnData={e} setSort={setActiveSort} activeSort={activeSort} activeFilters={activeFilters} setActiveFilters={setActiveFilters} />
                        ))
                    }
                </tr>
            </thead>
            <tbody className='datagrid_body'>
                {
                    data ? data.map((e) => <DatagridRow key={e.id} row={e} setHighlightedRows={setHighlightedRows} />)
                        :
                        <tr className='datagrid-row error'>
                            <td colSpan={columns.length}>
                                Данные не обнаружены!
                            </td>
                        </tr>
                }
            </tbody>
        </table>
    )
}

export default DataGridView