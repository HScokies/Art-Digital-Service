import { Button } from 'components/index'
import './style.scss'
import { ReactNode } from 'react'

interface props {
    title: string,
    dialog: HTMLDialogElement | null,
    children: ReactNode,
    form?: string
}
const DialogMenu = ({ title, dialog, children, form }: props) => {

    return (
        <div className='menu_dialog-wrapper'>
            <div className='menu_dialog-title'>
                {title}
            </div>
            <div className='menu_dialog-content'>
                {children}
            </div>
            <div className='menu_dialog-buttons'>
                <Button clickHandler={() => dialog?.close()} variant='passive' isActive={true}>Отмена</Button>
                <Button type='submit' form={form} isActive={true}>Сохранить</Button>
            </div>
        </div>
    )
}
export default DialogMenu