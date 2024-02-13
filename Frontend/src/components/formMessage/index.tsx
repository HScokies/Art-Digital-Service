import Icons from 'images/icons.svg'
import './style.scss'

interface props{
    isActive: boolean,
    type: 'error' | 'info'
    text: string
}

const FormMessage = ({isActive, text, type}: props) => {
    return(
        <div className={`form-message ${type} ${isActive? 'active' : ''}`}>
            <svg focusable="false" aria-hidden="true">                
                {
                   type == 'error'?
                   <use xlinkHref={Icons+'#error'} href={Icons+'#error'}/> :
                   <use xlinkHref={Icons+'#info'} href={Icons+'#info'}/>
                }
            </svg>
            <p>{text}</p>
        </div>
    )
}
export default FormMessage