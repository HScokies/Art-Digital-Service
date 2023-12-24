import Icons from 'images/icons.svg'
import { useEffect, useRef, useState } from 'react'
import { IColumnOptions, IHeaderCell, orderBy, param } from '../interfaces';



const HeaderCell = ({ id, title, setSort, activeSort, options, setFilters, activeFilters }: IHeaderCell) => {
    //#region handleSort
    const sortStates: (orderBy | undefined)[] = [
        undefined,
        {
            column: id,
            order: 'asc'
        },
        {
            column: id,
            order: 'desc'
        }
    ]
    const sortStatesIndex = useRef(0);
    useEffect(() => {
        if (activeSort?.column != id) {
            sortStatesIndex.current = 0;
        }
    }, [activeSort])

    const sortClassName = () => {
        if (activeSort?.column == id)
            return activeSort.order.toString()
        return ''
    }
    const handleSortChange = () => {
        sortStatesIndex.current = (sortStatesIndex.current + 1) % sortStates.length
        setSort(sortStates[sortStatesIndex.current])
    }
    //#endregion

    //#region handle options menu visibility
    const [menuActive, setMenuActive] = useState(false)
    useEffect(() => {
        const ToggleFiltersMenu = (e: MouseEvent) => {
            const element = (e.target) as HTMLElement
            if (element.id != id) {
                setMenuActive(false)
            }
        }
        document.addEventListener('click', ToggleFiltersMenu)
        return () => {
            document.removeEventListener('click', ToggleFiltersMenu)
        }
    }, [])
    //#endregion
    const [filtersActive, setFiltersActive] = useState(false)
    const handleOptionToggle = (param: param, status: boolean) => {
        const newFilters = activeFilters?.map((e) => {
            if (e.column == id && e.options) {
                const filter = e.options.find(o => o.id == param.id)
                if (filter) filter.isActive = status
                const isFiltered = e.options.find(o => !o.isActive)
                setFiltersActive(isFiltered?true : false)
            }
            return e
        })
        if (setFilters)
            setFilters(newFilters ? newFilters : [])
    }


    return (
        <th className='datagrid-cell header'>
            <div className='title-wrapper' onClick={() => handleSortChange()}>
                {title}
                <svg className={filtersActive ? 'active' : ''}>
                    <use xlinkHref={Icons + '#filter'} />
                </svg>

                <svg className={sortClassName()}>
                    <use xlinkHref={Icons + '#arrow_SM'} />
                </svg>
            </div>
            <div className='filters'>
                <span id={id} onClick={() => setMenuActive(true)} className={`datagrid-cell icon-wrapper ${options ? 'active' : ''}`}>
                    <svg id={id}>
                        <use id={id} xlinkHref={Icons + "#burger"} />
                    </svg>
                </span>
                <div className={`datagrid-cell menu ${menuActive ? 'active' : ''}`}>
                    {
                        options?.map((e) => {
                            const id = (Date.now() * Math.random()).toString(36);
                            return (
                                <div key={id} className='datagrid-cell menu-option' onClick={(e) => e.stopPropagation()}>
                                    <input checked={e.isActive} id={id} type='checkbox' onChange={(ev) => handleOptionToggle(e, ev.target.checked)} />
                                    <label htmlFor={id}>{e.title}</label>
                                </div>
                            )
                        })
                    }
                </div>
            </div>
        </th>
    )
}
export default HeaderCell