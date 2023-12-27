import './style.scss'
import HeaderCell from './HeaderCell'
import DatagridRow from './Row'
import { useEffect, useState } from 'react'
import { IColumn, IRow, filter, orderBy, pagination, param } from './interfaces'
import Icons from 'images/icons.svg'
import { Button } from 'components/index'

interface props {
    searchLabel: string,
    columns: IColumn[],
    source: (filters: param[], search?: string, orderBy?: orderBy) => IRow[],
    exportSource: (ids?: Set<number>) => void
    deleteSource: (ids: Set<number>) => void
}



const DataGridView = ({ columns, searchLabel, deleteSource, source, exportSource }: props) => {
    const dialog = document.getElementById('delete-row-dialog') as HTMLDialogElement
    const [data, setData] = useState<IRow[]>([])

    //array of highlighted rows ids
    const [highlightedRows, setHighlightedRows] = useState(new Set<number>([]))
    const [activeSort, setActiveSort] = useState<orderBy>()
    const [activeFilters, setActiveFilters] = useState(new Set<filter>([]))
    const [activeSearch, setActiveSearch] = useState<string>()
    const [pagination, setPagination] = useState<pagination>()

    //data handler
    useEffect(() => {

        let search = activeSearch?.trim()

        //exclude column from filter
        let params: param[] = []
        activeFilters.forEach((f) => params.push(f.param))

        const data = source(params, search, activeSort)
        setData(data)
    }, [activeSort, activeFilters, activeSearch])

    //highlight handler
    useEffect(() => {
        console.debug(highlightedRows)
        const rows = document.getElementsByClassName("datagrid-row")
        for (let i = 1; i < rows.length; i++) {
            const rowID = +rows[i].id.replace('rowid-', '')
            const checkbox = rows[i].querySelector('input') as HTMLInputElement
            if (!checkbox) continue;
            highlightedRows.has(rowID) ?
                checkbox.checked = true :
                checkbox.checked = false
        }
    }, [highlightedRows, pagination])

    /*
    TODO:
        Make a datagrid wrapper w/ top & bottom menu
            bottom menu inside tfoot?
            top menu: searchbar, insert?, refresh?, csv export
                if highlighted > 0 => delete, export to csv selected
                    "X btn", N Записей выделено | Экспортировать, Удалить
                
        Add expand button w/ aside menu for entity change
        
        top menu:
        search, insert, export [delete, export selected]

        bottom menu:
            pagination
            rows per page, current page of total pages, nav arrows
        
    */
    const handleDelete = () => {
        deleteSource(highlightedRows)
        dialog.close() 
    }

    const renderDeleteDialog = () => {
        return (
            <dialog id='delete-row-dialog'>
                <div className="title">
                    Подтвердите удаление выбранных строк
                </div>
                <div className='descr'>
                    <p>Вы уверены, что хотите удалить выбранные строки?</p>
                    <p>Это действие не может быть отменено.</p>
                </div>
                <div className='button-container'>
                    <Button variant='passive' isActive={true} clickHandler={() => dialog.close()}>Отмена</Button>
                    <Button clickHandler={() => handleDelete()} variant='danger' isActive={true}>Удалить</Button>
                </div>
            </dialog>
        )
    }

    const renderTopMenu = () => {
        return (
            highlightedRows.size > 0 ?
                <div className='highlighted-menu'>
                    <div className='highlighted-menu rows-controll'>
                        <button className='rows-controll-btn' onClick={() => setHighlightedRows(new Set<number>([]))}>
                            <svg>
                                <use xlinkHref={Icons + '#close_md'} />
                            </svg>
                        </button>
                        <span className='rows-controll-label'>Выделено строк: {highlightedRows.size}</span>
                    </div>
                    <div className='btn-wrapper'>
                        <Button variant='brand' isActive={true} clickHandler={() => exportSource(highlightedRows)}>Экспорт</Button>
                        <Button variant='passive' isActive={true} clickHandler={() => dialog.showModal()}>Удалить</Button>
                    </div>
                </div>
                :
                <>
                    <input className='searchbar' onChange={(e) => setActiveSearch(e.target.value)} type='search' placeholder={searchLabel} />
                    <div className='btn-wrapper'>
                        <Button isActive={true}>Добавить</Button>
                        <Button variant='passive' isActive={true} clickHandler={() => exportSource()}>Экспорт</Button>
                    </div>
                </>
        )
    }
    return (
        <>
            <div className='datagrid-wrapper'>
                <div className='datagrid_menu-top'>
                    {renderTopMenu()}
                </div>
                <div className='tableWrapper'>
                    <table className='datagrid'>
                        <thead className='datagrid_header'>
                            <tr className='datagrid-row'>
                                <th className='row-select-header'></th>
                                {
                                    columns.map((e, i) => (
                                        <HeaderCell key={e.id} index={i} columnData={e} setSort={setActiveSort} activeSort={activeSort} activeFilters={activeFilters} setActiveFilters={setActiveFilters} />
                                    ))
                                }
                            </tr>
                        </thead>
                        <tbody className='datagrid_body'>
                            {
                                data && data.length > 0 ? data.map((e) => <DatagridRow key={e.id} row={e} setHighlightedRows={setHighlightedRows} />)
                                    :
                                    <tr className='datagrid-row error'>
                                        <td colSpan={columns.length + 1}>
                                            Данные не обнаружены!
                                        </td>
                                    </tr>
                            }
                        </tbody>
                    </table>
                </div>
                <h2>footer</h2>
            </div>
            {renderDeleteDialog()}
        </>

    )
}

export default DataGridView