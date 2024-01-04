import { ReactNode } from 'react'
import './style.scss'

interface props{
    children: ReactNode,
    id?: string,
    type?: 'button' | 'reset' | 'submit' | undefined,
    form?: string,
    variant?: 'passive' | 'brand' | 'danger'
    isActive?: boolean,
    clickHandler?: () => void
}

const Button = ({children, type, form, id, variant='brand', clickHandler = () => {}, isActive}: props) => (
    <button id={id} form={form} type={type} className={`button ${variant} ${isActive?'':'disabled'}`} disabled={isActive? undefined : true} onClick={isActive? () => clickHandler() : undefined}>
        {children}
    </button>
)

export default Button