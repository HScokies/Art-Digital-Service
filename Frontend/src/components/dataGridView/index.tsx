import './style.scss'
import HeaderCell from './HeaderCell'
import DatagridRow from './Row'
import { useEffect, useState } from 'react'
import { IColumn, IData, filter, orderBy, param } from './interfaces'
import { BottomMenu, TopMenu } from './Menu'
import { DeleteDialog, FormDialog } from './Modals'
import { formProps } from './Modals/createUpdateDialog'

interface props {
    searchLabel: string,
    columns: IColumn[],
    rowsPerPageOptions: Set<number>
    dataSource: (offset: number, take: number, filters: param[], search?: string, orderBy?: orderBy) => IData,
    exportProvider: (ids?: Set<number>) => void
    deleteProvider?: (ids: Set<number>) => void,
    updateProvider: (id: number, model: FormData) => void,
    createProvider?: (model: FormData) => void,
    createForm?: () => JSX.Element,
    updateForm: ({ id }: formProps) => JSX.Element
}



const DataGridView = ({ columns, rowsPerPageOptions, dataSource, searchLabel, exportProvider, deleteProvider, createProvider, updateProvider, createForm, updateForm }: props) => {
    const [data, setData] = useState<IData>()

    //array of highlighted rows ids
    const [highlightedRows, setHighlightedRows] = useState(new Set<number>([]))
    const [activeSort, setActiveSort] = useState<orderBy>()
    const [activeFilters, setActiveFilters] = useState(new Set<filter>([]))
    const [activeSearch, setActiveSearch] = useState<string>()

    const [rowsPerPage, setRowsPerPage] = useState([...rowsPerPageOptions][0] || 0)
    const [currentPage, setCurrentPage] = useState(1)

    const [currentRowId, setCurrentRowId] = useState(-1)

    //Handle data
    useEffect(() => {
        let search = activeSearch?.trim()

        //exclude column from filter
        let params: param[] = []
        activeFilters.forEach((f) => params.push(f.param))

        const offset = (currentPage - 1) * rowsPerPage;
        const fetch = async() => {
            const data = await dataSource(offset, rowsPerPage, params, search, activeSort)
            if (data.currentPage > 0)
                setCurrentPage(data.currentPage)
            else setCurrentPage(1)
            setData(data)
        }
        fetch();

    }, [activeSort, activeFilters, activeSearch, rowsPerPage, currentPage])

    // handle highlightedRows
    useEffect(() => {
        const rows = document.getElementsByClassName("datagrid-row")
        for (let i = 1; i < rows.length; i++) {
            const rowID = +rows[i].id.replace('rowid-', '')
            const checkbox = rows[i].querySelector('input') as HTMLInputElement
            if (!checkbox) continue;
            highlightedRows.has(rowID) ?
                checkbox.checked = true :
                checkbox.checked = false
        }
    }, [highlightedRows, currentPage])





    return (
        <>
            <div className='datagrid-wrapper'>
                <TopMenu
                    highlightedRows={highlightedRows}
                    setHighlightedRows={setHighlightedRows}
                    exportProvider={exportProvider}
                    searchLabel={searchLabel}
                    setSearch={setActiveSearch}
                />
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
                                data && data.participants?.length > 0 ? data.participants.map((e) => <DatagridRow key={e.id} row={e} setCurrentRowId={setCurrentRowId} setHighlightedRows={setHighlightedRows} />)
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
                <BottomMenu
                    currentPage={currentPage}
                    setPage={setCurrentPage}
                    setRowsPerPage={setRowsPerPage}
                    rowsPerPageOptions={rowsPerPageOptions}
                    pageCount={data?.pagesTotal && data.pagesTotal > 0? data?.pagesTotal : 1}
                />
            </div>
            {
                deleteProvider &&
                <DeleteDialog
                    deleteProvider={deleteProvider}
                    highlightedRows={highlightedRows}
                />
            }
            {
                (createProvider && createForm) &&
                <FormDialog
                    dialogId='create'
                    onSubmit={createProvider}
                    dialogTitle='Создание записи'
                    FormElements={createForm}
                />
            }
            <FormDialog
                dialogId='update'
                onSubmit={updateProvider}
                dialogTitle='Обновление записи'
                rowId={currentRowId}
                setActiveRow={setCurrentRowId}
                FormElements={updateForm}
            />
        </>

    )
}

export default DataGridView