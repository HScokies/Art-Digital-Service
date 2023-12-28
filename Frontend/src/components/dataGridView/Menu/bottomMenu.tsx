import './style.scss'
import Icons from 'images/icons.svg'
import { useEffect, useState } from 'react'

interface props {
    setRowsPerPage: React.Dispatch<React.SetStateAction<number>>,
    setPage: React.Dispatch<React.SetStateAction<number>>,
    rowsPerPageOptions: Set<number>,
    currentPage: number,    
    pageCount: number
}
const BottomMenu = ({ setRowsPerPage, setPage, currentPage, rowsPerPageOptions, pageCount }: props) => {
    
    useEffect(() => {
        handleBlur(currentPage)
    }, [pageCount])
    
    //page validator
    const handleBlur = (Page: number) => {
        if (Page > pageCount)
            Page = pageCount
        if (Page < 1)
            Page = 1
        setPage(Page)
    }

    const handlePageChange = (type: 'inc' | 'dec') => {
        if (type=='inc' && currentPage<pageCount){
            setPage(currentPage+1)
        }
        else if (currentPage>1){
            setPage(currentPage-1)
        }
    }
    return (
        <div className='datagrid_menu-bottom'>
            <div className='container'>
                <span className='bottom-menu-label'>Записей на странице:</span>
                <select className='bottom-menu-dropdown'>
                    {
                        [...rowsPerPageOptions].map((o) => (
                            <option key={o} onClick={() => setRowsPerPage(o)}>{o}</option>
                        ))
                    }
                </select>
            </div>

            <div className='container'>
                <span className='bottom-menu-label'>
                    Страница
                    <input min={1} max={pageCount} type='number' value={currentPage} onChange={(e) => setPage(+e.target.value)} onBlur={(e) => handleBlur(+e.target.value)} />
                    из {pageCount}
                </span>
            </div>

            <div className='container'>
                <button id='datagrid-prev' disabled={currentPage==1} onClick={() => handlePageChange('dec')}>
                    <svg>
                        <use xlinkHref={Icons + '#downarrow'} />
                    </svg>
                </button>

                <button id='datagrid-next' disabled={currentPage==pageCount} onClick={() => handlePageChange('inc')}>
                    <svg>
                        <use xlinkHref={Icons + '#downarrow'} />
                    </svg>
                </button>
            </div>
        </div>
    )
}
export default BottomMenu