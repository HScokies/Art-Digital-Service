
import Icons from 'images/icons.svg'
import { IRow } from "../interfaces";

interface props {
    row: IRow,
    setHighlightedRows: React.Dispatch<React.SetStateAction<Set<number>>>,
    setCurrentRowId: React.Dispatch<React.SetStateAction<number>>
}

const DatagridRow = ({ row, setHighlightedRows, setCurrentRowId }: props) => {
    const updateDialog = document.getElementById("dialog-update") as HTMLDialogElement

    const toggleHighlighted = (isChecked: boolean) => {
        !isChecked ?
            setHighlightedRows(rows => {
                rows.delete(row.id)
                return new Set(rows)
            }) : setHighlightedRows(rows => new Set(rows.add(row.id)))
    }

    return (
        <tr className='datagrid-row' id={`rowid-${row.id}`}>
            <td className="datagrid-cell">
                <div className="controll">
                    <input onChange={(e) => toggleHighlighted(e.target.checked)} type='checkbox' />
                    <button className='datagrid-expand' onClick={() => {setCurrentRowId(row.id); updateDialog.showModal()}}>
                        <svg>
                            <use xlinkHref={Icons + "#expand"} />
                        </svg>
                    </button>
                </div>
            </td>
            {
                Object.entries(row).slice(1).map((e, col) => (
                    <td className={`datagrid-cell col_${col}`} key={col}>{e[1]}</td>
                ))
            }
        </tr>
    )
}

export default DatagridRow