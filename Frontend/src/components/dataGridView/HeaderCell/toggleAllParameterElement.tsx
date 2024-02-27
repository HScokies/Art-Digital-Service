import { useEffect, useState } from "react"

interface props {
    filterStates: boolean[]
    updateFilters: (states: boolean[]) => void
}
const ToggleAllParameterElement = ({ filterStates, updateFilters }: props) => {
    const [id] = useState((Date.now() * Math.random()).toString(36))

    const [isActive, setActive] = useState(true);
    useEffect(() => {
        for (let filter of filterStates) {
            if (filter === false)
                return setActive(false)
        }
        setActive(true)
    }, [filterStates])

    const toggleFilters = (state: boolean) => {
        const newFilterStates = filterStates;
        for (let i=0; i<newFilterStates.length; i++)
            newFilterStates[i] = state;

        updateFilters(newFilterStates)

        setActive(state)
    }

    return (
        <div className='datagrid-cell menu-option select-all' onClick={(e) => e.stopPropagation()}>
            <input checked={isActive} id={id} type='checkbox' onChange={(e) => toggleFilters(e.target.checked)} />
            <label htmlFor={id}>Выбрать все</label>
        </div>
    )
}

export default ToggleAllParameterElement