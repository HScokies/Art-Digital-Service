import './style.scss'

interface props{
    children: string,
    clickHandler?: () => void
}

const Button = ({children, clickHandler = () => {}}: props) => (
    <a className='button' onClick={() => clickHandler()}>
        {children}
    </a>
)

export default Button