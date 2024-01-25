import './style.scss'
import Icons from 'images/icons.svg'
import { useEffect, useState } from 'react'
import { Button } from 'src/components'

interface props {
    highlightedRows: Set<number>,
    setHighlightedRows: React.Dispatch<React.SetStateAction<Set<number>>>,
    exportProvider: (ids?: Set<number>) => string,
    searchLabel: string,
    setSearch: React.Dispatch<React.SetStateAction<string | undefined>>
}

const TopMenu = ({ highlightedRows, setHighlightedRows, exportProvider, searchLabel, setSearch }: props) => {
    const deleteDialog = document.getElementById('delete-row-dialog') as HTMLDialogElement
    const createDialog = document.getElementById('dialog-create') as HTMLDialogElement
    const [exportUrl, setUrl] = useState("");
    useEffect(() => {
        const url = exportProvider(highlightedRows);
        setUrl(url)
    }, [highlightedRows])

    const download = () => {
        console.debug(exportUrl)
        location.href = exportUrl;
    }
    return (
        <div className='datagrid_menu-top'>
            {
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
                            <Button variant='brand' isActive={true} clickHandler={() => download()}>Экспорт</Button>
                            {
                                deleteDialog&&
                                <Button variant='passive' isActive={true} clickHandler={() => deleteDialog.showModal()}>Удалить</Button>
                            }                            
                        </div>
                    </div>
                    :
                    <>
                        <input className='searchbar' onChange={(e) => setSearch(e.target.value)} type='search' placeholder={searchLabel} />
                        <div className='btn-wrapper'>
                            {
                                createDialog&&
                                <Button isActive={true} clickHandler={() => createDialog.showModal()}>Добавить</Button>
                            }                            
                            <Button variant='passive' isActive={true} clickHandler={() => download()}>Экспорт</Button>
                        </div>
                    </>
            }
        </div>
    )
}
export default TopMenu