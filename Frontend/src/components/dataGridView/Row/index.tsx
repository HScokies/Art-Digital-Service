import { useState } from "react";
import { IRow } from "../interfaces";

interface props {
    row: IRow,
    setHighlightedRows: React.Dispatch<React.SetStateAction<Set<number>>>
}

const DatagridRow = ({ row, setHighlightedRows }: props) => {
    const toggleHighlighted = (isChecked: boolean) => {
        !isChecked ?
            setHighlightedRows(rows => {
                rows.delete(row.id)
                return new Set(rows)
            }) : setHighlightedRows(rows => new Set(rows.add(row.id)))
    }

    return (
        <tr className='datagrid-row' id={`rowid-${row.id}`}>
            <td><input onChange={(e) => toggleHighlighted(e.target.checked)} type='checkbox' /></td>
            {
                Object.entries(row.data).map((e, col) => (
                    <td className={`datagrid-cell col_${col}`} key={col}>{e[1]}</td>
                ))
            }
        </tr>
    )
}

export default DatagridRow