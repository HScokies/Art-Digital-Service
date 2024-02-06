import './style.scss'
import Icons from 'images/icons.svg'
import { useEffect, useState } from 'react'
import { Button } from 'src/components'

interface props {
    highlightedRows: Set<number>,
    setHighlightedRows: React.Dispatch<React.SetStateAction<Set<number>>>,
    exportProvider: (ids?: Set<number>) => string,
    searchLabel: string,
    setSearch: React.Dispatch<React.SetStateAction<string | undefined>>,
    onCreate?: () => void
    onDelete?: () => void
}

const TopMenu = ({ highlightedRows, setHighlightedRows, exportProvider, searchLabel, setSearch, onCreate, onDelete }: props) => {
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
                                onDelete&&
                                <Button variant='passive' isActive={true} clickHandler={() => onDelete()}>Удалить</Button>
                            }                            
                        </div>
                    </div>
                    :
                    <>
                        <input className='searchbar' onChange={(e) => setSearch(e.target.value)} type='search' placeholder={searchLabel} />
                        <div className='btn-wrapper'>
                            {
                                onCreate&&
                                <Button isActive={true} clickHandler={() => onCreate()}>Добавить</Button>//createDialog.showModal()
                            }                            
                            <Button variant='passive' isActive={true} clickHandler={() => download()}>Экспорт</Button>
                        </div>
                    </>
            }
        </div>
    )
}
export default TopMenu