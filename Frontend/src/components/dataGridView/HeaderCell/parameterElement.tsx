import { useEffect, useState } from "react"
import { IColumn, filter, param } from "../interfaces"

interface props{
    parent: string,
    parameter: param,
    setHasActiveFilters: React.Dispatch<React.SetStateAction<boolean>>,
    activeFilters: Set<filter>,
    setActiveFilters: React.Dispatch<React.SetStateAction<Set<filter>>>
}

const ParameterElement = ({parent, parameter, setHasActiveFilters, activeFilters, setActiveFilters}: props) => {
    const [id] = useState((Date.now() * Math.random()).toString(36))
    const [filter] = useState<filter>({column: parent, param: parameter})
    const [isDisplayed, setIsDisplayed] = useState(true)

    useEffect(() => {
        const toggleHasActiveFilters = () => {
            for (let filter of activeFilters){
                if (filter.column == parent){
                    return setHasActiveFilters(true)
                }
            }    
            return setHasActiveFilters(false)
        }
        toggleHasActiveFilters()
    }, [activeFilters])

    const toggleFilter = async(isActive: boolean) => {
        await isActive? setActiveFilters(filters => {
            filters.delete(filter)
            return new Set(filters)
        }) : setActiveFilters(filters => new Set(filters.add(filter)))
        setIsDisplayed(isActive)
    }

    return (
        <div className='datagrid-cell menu-option' onClick={(e) => e.stopPropagation()}>
            <input checked={isDisplayed} id={id} type='checkbox' onChange={(e) => toggleFilter(e.target.checked)} />
            <label htmlFor={id}>{parameter.title}</label>
        </div>
    )
}

export default ParameterElement