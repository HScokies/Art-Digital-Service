import { ReactNode } from 'react'
import './style.scss'

interface props{
    children: ReactNode,
    id?: string,
    variant?: 'passive' | 'brand' | 'danger'
    isActive?: boolean,
    clickHandler?: () => void
}

const Button = ({children, id, variant='brand', clickHandler = () => {}, isActive}: props) => (
    <button id={id} className={`button ${variant} ${isActive?'':'disabled'}`} disabled={isActive? undefined : true} onClick={isActive? () => clickHandler() : undefined}>
        {children}
    </button>
)

export default Button