import { Button } from "src/components"

interface props{
    deleteProvider: (ids: Set<number>) => void
    highlightedRows: Set<number>
}
const DeleteDialog = ({deleteProvider, highlightedRows}: props) => {
    const dialog = document.getElementById('delete-row-dialog') as HTMLDialogElement
    
    const handleDelete = () => {
        deleteProvider(highlightedRows)
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