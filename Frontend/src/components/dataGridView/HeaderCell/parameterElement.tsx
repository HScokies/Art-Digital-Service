import { useEffect, useState } from "react"
import { filter, param } from "../interfaces"

interface props {
    // defaultState: boolean,
    parent: string,
    parameter: param,
    setHasActiveFilters: React.Dispatch<React.SetStateAction<boolean>>,
    activeFilters: Set<filter>,
    setActiveFilters: React.Dispatch<React.SetStateAction<Set<filter>>>,

    toggleActive: (state: boolean) => void,
    isActive: boolean
}

const ParameterElement = ({ parent, parameter, setHasActiveFilters, activeFilters, setActiveFilters, toggleActive, isActive = true }: props) => {
    const [id] = useState((Date.now() * Math.random()).toString(36))
    const [filter] = useState<filter>({ column: parent, param: parameter })

    useEffect(() => {
        const toggleHasActiveFilters = () => {
            for (let filter of activeFilters) {
                if (filter.column == parent) {
                    return setHasActiveFilters(true)
                }
            }
            return setHasActiveFilters(false)
        }
        toggleHasActiveFilters()
    }, [activeFilters])

    useEffect(() => {
        isActive? setActiveFilters(filters => {
            filters.delete(filter)
            return new Set(filters)
        }) : setActiveFilters(filters => new Set(filters.add(filter)))
    }, [isActive])

    return (
        <div className='datagrid-cell menu-option' onClick={(e) => e.stopPropagation()}>
            <input checked={isActive} id={id} type='checkbox' onChange={(e) => toggleActive(e.target.checked)} />
            <label htmlFor={id}>{parameter.title}</label>
        </div>
    )
}

export default ParameterElement