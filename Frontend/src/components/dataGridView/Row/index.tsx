import { useState } from "react";
import { IRow } from "../interfaces";

interface props {
    row: IRow,
    setHighlightedRows: React.Dispatch<React.SetStateAction<Set<Number>>>
}

const DatagridRow = ({ row, setHighlightedRows }: props) => {
    const [isHighlighted, setHighlighted] = useState(false)
    const toggleHighlighted = () => {
        isHighlighted ?
            setHighlightedRows(rows => {
                rows.delete(row.id)
                return new Set(rows)
            }) : setHighlightedRows(rows => new Set(rows.add(row.id)))

        setHighlighted(!isHighlighted)
    }

    return (
        <tr className='datagrid-row'>
            <td><input checked={isHighlighted} onChange={() => toggleHighlighted()} type='checkbox' /></td>
            {
                Object.entries(row.data).map((e, col) => (
                    <td className={`datagrid-cell col_${col}`} key={col}>{e[1]}</td>
                ))
            }
        </tr>
    )
}

export default DatagridRow