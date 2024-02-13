import Icons from 'images/icons.svg'
import { useEffect, useRef, useState } from 'react'
import { IHeaderCell, orderBy } from '../interfaces';
import ParameterElement from './parameterElement';



const HeaderCell = ({ index, sizePx, columnData, setSort, activeSort, setActiveFilters, activeFilters }: IHeaderCell) => {
    //#region handleSort
    const sortStates: (orderBy | undefined)[] = [
        undefined,
        {
            column: columnData.id,
            asc: true
        },
        {
            column: columnData.id,
            asc: false
        }
    ]
    const sortStatesIndex = useRef(0);
    useEffect(() => {
        if (activeSort?.column != columnData.id) {
            sortStatesIndex.current = 0;
        }
    }, [activeSort])

    const sortClassName = () => {
        if (activeSort?.column == columnData.id)
            return activeSort.asc? 'asc' : 'desc';
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
            if (element.id != columnData.id) {
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


    return (
        <th className={`datagrid-cell col_${index}`} style={{width: sizePx}}>
            <div className='header'>
            <div className='title-wrapper' onClick={() => handleSortChange()}>
                {columnData.title}
                <svg className={filtersActive ? 'active' : 'inactive'}>
                    <use xlinkHref={Icons + '#filter'} href={Icons + '#filter'} />
                </svg>

                <svg className={sortClassName()}>
                    <use xlinkHref={Icons + '#arrow_SM'} href={Icons + '#arrow_SM'} />
                </svg>
            </div>
            <div className='filters'>
                <span id={columnData.id} className={`datagrid-cell icon-wrapper ${columnData.filters ? 'active' : ''}`}>
                    <svg id={columnData.id} onClick={() => setMenuActive(true)}>
                        <use id={columnData.id} xlinkHref={Icons + "#burger"} href={Icons + "#burger"} />
                    </svg>
                </span>
                <div className={`datagrid-cell menu ${menuActive ? 'active' : ''}`}>
                    {
                        columnData.filters?.map((e, i) => {
                            return (
                                <ParameterElement key={i} parent={columnData.id} setHasActiveFilters={setFiltersActive} parameter={e} activeFilters={activeFilters} setActiveFilters={setActiveFilters} />
                            )
                        })
                    }
                </div>
            </div>
            </div>
        </th>
    )
}
export default HeaderCell