import './style.scss'

interface props{
    children: string,
    isActive?: boolean,
    clickHandler?: () => void
}

const Button = ({children, clickHandler = () => {}, isActive}: props) => (
    <a className={isActive? 'button' : 'button disabled'} onClick={() => clickHandler()}>
        {children}
    </a>
)

export default Button