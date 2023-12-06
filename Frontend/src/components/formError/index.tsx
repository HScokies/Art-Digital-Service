import './style.scss'

interface props{
    isActive: boolean,
    text: string
}

const FormError = ({isActive, text}: props) => {

    return(
        <div className={`form-error ${isActive? 'active' : ''}`}>
            <svg focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path fill-rule="evenodd" clip-rule="evenodd" d="M12 20a8 8 0 100-16 8 8 0 000 16zm2.293-4.293L12 13.414l-2.293 2.293-1.414-1.414L10.586 12 8.293 9.707l1.414-1.414L12 10.586l2.293-2.293 1.414 1.414L13.414 12l2.293 2.293-1.414 1.414z"></path></svg>
            <p>{text}</p>
        </div>
    )
}
export default FormError