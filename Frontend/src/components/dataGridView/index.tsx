import './style.scss'
import HeaderCell from './HeaderCell'
import DatagridRow from './Row'
import { useEffect, useRef, useState } from 'react'
import { IColumn, IData, filter, orderBy, param } from './interfaces'
import { BottomMenu, TopMenu } from './Menu'
import { DialogConfirm, DialogMenu } from 'components/index'
import { ICreateForm, IUpdateForm } from 'src/interfaces'


interface props {
    searchLabel: string,
    columns: IColumn[],
    rowsPerPageOptions: Set<number>
    dataSource: (offset: number, take: number, filters: param[], search?: string, orderBy?: orderBy) => Promise<IData>,
    exportProvider: (ids?: Set<number>) => string
    deleteProvider?: (ids: Set<number>) => void,
    CreateForm?: React.FC<ICreateForm>,
    UpdateForm?: React.FC<IUpdateForm>
}



const DataGridView = ({ columns, rowsPerPageOptions, dataSource, searchLabel, exportProvider, deleteProvider, CreateForm, UpdateForm }: props) => {
    const [data, setData] = useState<IData>()

    //array of highlighted rows ids
    const [highlightedRows, setHighlightedRows] = useState(new Set<number>([]))
    const [activeSort, setActiveSort] = useState<orderBy>()
    const [activeFilters, setActiveFilters] = useState(new Set<filter>([]))
    const [activeSearch, setActiveSearch] = useState<string>()

    const [rowsPerPage, setRowsPerPage] = useState([...rowsPerPageOptions][0] || 0)
    const [currentPage, setCurrentPage] = useState(1)

    const [_trigger, setTrigger] = useState(false);
    const trigger = () => setTrigger(!_trigger);

    //Handle data
    useEffect(() => {
        let search = activeSearch?.trim()

        //exclude column from filter
        let params: param[] = []
        activeFilters.forEach((f) => params.push(f.param))

        const offset = (currentPage - 1) * rowsPerPage;
        const fetch = async () => {
            const data = await dataSource(offset, rowsPerPage, params, search, activeSort)
            if (data.currentPage > 0)
                setCurrentPage(data.currentPage)
            else setCurrentPage(1)
            setData(data)
        }
        fetch();

    }, [activeSort, activeFilters, activeSearch, rowsPerPage, currentPage, _trigger])

    // handle highlightedRows
    useEffect(() => {
        const rows = document.getElementsByClassName("datagrid-row")
        for (let i = 1; i < rows.length; i++) {
            const rowID = +(rows[i].id.replace('rowid-', ''))
            const element = rows[i].querySelector('input') as HTMLInputElement;
            if (element)
                element.checked = highlightedRows.has(rowID);
        }
    }, [highlightedRows, data])

    //#region Update / Create dialog
    const upsertDialog = useRef<HTMLDialogElement>(null)
    const [upsertFormTitle, setUpsertFormTitle] = useState("")
    const [upsertFormId, setUpsertFormId] = useState<string>()
    const [upsertForm, setUpsertForm] = useState<React.JSX.Element>()

    const showUpsertForm = (title: string, id?: string, form?: React.JSX.Element) => {
        setUpsertFormTitle(title)
        setUpsertFormId(id)
        setUpsertForm(form)
        upsertDialog.current?.showModal()
    }
    const OpenCreateDialog = () => {
        if (!CreateForm) return;
        const formID = "CreateEntityForm";
        showUpsertForm('Создание записи', formID, <CreateForm formId={formID} />)
    }
    const OpenUpdateDialog = (id: number) => {
        if (!UpdateForm) return;
        const formID = "updateEntityForm";
        showUpsertForm('Обновление записи', formID, <UpdateForm formId={formID} entityId={id} />)
    }
    useEffect(() => {
        const onClose = () => {
            setUpsertForm(undefined)
            trigger()
        }
        upsertDialog.current?.addEventListener('close', onClose)
    }, [])
    //#endregion

    //#region  Delete dialog
    const deleteDialog = useRef<HTMLDialogElement>(null)
    const onDelete = async () => {
        if (!deleteProvider) return
        await deleteProvider(highlightedRows)
        setHighlightedRows(new Set<number>)
        deleteDialog.current?.close()
    }
    
    useEffect(() => {
        deleteDialog.current?.addEventListener('close', trigger)
    }, [])
    //#endregion
    return (
        <>
            <div className='datagrid-wrapper'>
                <TopMenu
                    onCreate={CreateForm && (() => OpenCreateDialog())}
                    onDelete={deleteProvider && (() => deleteDialog.current?.showModal())}
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
                                <th className='row-select-header' style={{ width: 136 }}></th>
                                {
                                    columns.map((e, i) => (
                                        <HeaderCell key={e.id} index={i} sizePx={e.sizePx} columnData={e} setSort={setActiveSort} activeSort={activeSort} activeFilters={activeFilters} setActiveFilters={setActiveFilters} />
                                    ))
                                }
                            </tr>
                        </thead>
                        <tbody className='datagrid_body'>
                            {
                                data && data.rows?.length > 0 ? data.rows.map((e) => <DatagridRow key={e.id} row={e} setHighlightedRows={setHighlightedRows} onExpand={() => OpenUpdateDialog(e.id)} />)
                                    :
                                    <tr className='datagrid-row error'>
                                        <td colSpan={columns.length + 1}>
                                            Данные не обнаружены!
                                        </td>
                                    </tr>
                            }
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colSpan={columns.length + 1}>
                                    <BottomMenu
                                        currentPage={currentPage}
                                        setPage={setCurrentPage}
                                        setRowsPerPage={setRowsPerPage}
                                        rowsPerPageOptions={rowsPerPageOptions}
                                        pageCount={data?.pagesTotal && data.pagesTotal > 0 ? data?.pagesTotal : 1}
                                    />
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
            {
                deleteProvider &&
                <dialog ref={deleteDialog} className='datagrid_delete-dialog'>
                    <DialogConfirm title='Подтвердите удаление выбранных строк' acceptText='Удалить' acceptStyle='danger' dialog={deleteDialog.current} onAccept={() => onDelete()}>
                        <p>Вы уверены, что хотите удалить выбранные строки?</p>
                        <p>Это действие не может быть отменено.</p>
                    </DialogConfirm>
                </dialog>
            }
            <dialog ref={upsertDialog} className='datagrid_upsert-dialog'>
                <DialogMenu title={upsertFormTitle} dialog={upsertDialog.current} form={upsertFormId}>
                    {upsertForm}
                </DialogMenu>
            </dialog>
        </>

    )
}

export default DataGridView