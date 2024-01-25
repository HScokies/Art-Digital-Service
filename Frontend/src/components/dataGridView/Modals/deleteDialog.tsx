import './style.scss'
import { Button } from "src/components"

interface props{
    deleteProvider: (ids: Set<number>) => void
    highlightedRows: Set<number>,
    setHighlighted: React.Dispatch<React.SetStateAction<Set<number>>>,
    trigger: () => void
}
const DeleteDialog = ({deleteProvider, highlightedRows, setHighlighted, trigger}: props) => {
    const dialog = document.getElementById('delete-row-dialog') as HTMLDialogElement
    
    const handleDelete = async() => {
        await deleteProvider(highlightedRows)
        setHighlighted(new Set<number>)
        trigger();
        dialog.close()
    }

    return (
        <dialog id='delete-row-dialog'>
            <div className="title">
                Подтвердите удаление выбранных строк
            </div>
            <div className='descr'>
                <p>Вы уверены, что хотите удалить выбранные строки?</p>
                <p>Это действие не может быть отменено.</p>
            </div>
            <div className='button-container'>
                <Button variant='passive' clickHandler={() => dialog.close()} isActive={true}>Отмена</Button>
                <Button clickHandler={() => handleDelete()} variant='danger' isActive={true}>Удалить</Button>
            </div>
        </dialog>
    )
}

export default DeleteDialog