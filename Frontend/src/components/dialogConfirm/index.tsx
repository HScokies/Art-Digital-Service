import { ReactNode } from 'react'
import './style.scss'
import { Button } from 'components/index'

interface props {
    title: string,
    acceptText: string,
    children: ReactNode,
    acceptStyle?: 'passive' | 'brand' | 'danger',
    dialog: HTMLDialogElement | null
    onAccept: () => void,

}
const DialogConfirm = ({ title, acceptText, children, acceptStyle, dialog, onAccept }: props) => (
    <div className='confirm_dialog-wrapper'>
        <div className="confirm_dialog-title">
            {title}
        </div>
        <div className='confirm_dialog-content'>
            {children}
        </div>
        <div className='confirm_dialog-buttons'>
            <Button variant='passive' clickHandler={() => dialog?.close()} isActive={true}>Отмена</Button>
            <Button clickHandler={() => onAccept()} variant={acceptStyle||'brand'} isActive={true}>{acceptText}</Button>
        </div>
    </div>
)
export default DialogConfirm;