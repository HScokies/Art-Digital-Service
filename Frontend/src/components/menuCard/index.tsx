import './style.scss'
import Icons from 'images/icons.svg'

interface props {
    title: string,
    onExpand: () => void,
    onDelete?: () => void
}
const MenuCard = ({ title, onExpand, onDelete }: props) => {

    return(
    <div className='menuCard-wrapper'>
        {title}
        <span className='menuCard-iconwrapper'>
            <svg onClick={() => onExpand()}>
                <use xlinkHref={Icons + '#expand'} href={Icons + '#expand'} />
            </svg>
            {
                onDelete &&
                <svg className="drop" onClick={() => onDelete()}>
                    <use xlinkHref={Icons + "#trash"} href={Icons + "#trash"} />
                </svg>
            }
        </span>
    </div>
    )
}
export default MenuCard