import { useEffect } from 'react'
import './style.scss'
import { Button } from 'src/components'

export interface formProps {
    id?: number
}

interface props {
    dialogId: string,
    dialogTitle: string,
    setActiveRow?: React.Dispatch<React.SetStateAction<number>>,
    rowId?: number
    FormElements: ({ id }: formProps) => JSX.Element,
    onSubmit: Function
}

const FormDialog = ({ dialogId, dialogTitle, rowId, FormElements, onSubmit, setActiveRow }: props) => {
    const thisDialog = document.getElementById(`dialog-${dialogId}`) as HTMLDialogElement
    useEffect(() => {
        const closeHandler = () => {
            if (setActiveRow) 
                setActiveRow(-1)
        }
        thisDialog?.addEventListener('close', closeHandler)
    }, [thisDialog])
    
    

    const submitHandler = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const data = new FormData(e.target as HTMLFormElement)
        if (rowId != undefined){
            onSubmit(rowId, data)
        } else onSubmit(data)
    }
    
    return (
        <dialog id={`dialog-${dialogId}`} className='aside-dialog'>
            <div className='aside-dialog-wrapper'>
                <div className='aside-dialog-title'>
                    {dialogTitle}
                </div>
                <form id={`aside-menu-form-${dialogId}`} className='aside-dialog-form' onSubmit={(e) => submitHandler(e)}>
                    <FormElements id={rowId} />
                </form>
                <div className='aside-dialog-buttons'>
                    <Button clickHandler={() => thisDialog.close()} variant='passive' isActive={true}>Отмена</Button>
                    <Button type='submit' form={`aside-menu-form-${dialogId}`} isActive={true}>Сохранить</Button>
                </div>
            </div>
        </dialog>
    )
}
export default FormDialog