import './style.scss'

interface props{
    children: string,
    isActive?: boolean,
    clickHandler?: () => void
}

const Button = ({children, clickHandler = () => {}, isActive}: props) => (
    <button className={isActive? 'button' : 'button disabled'} onClick={isActive? () => clickHandler() : undefined}>
        {children}
    </button>
)

export default Button