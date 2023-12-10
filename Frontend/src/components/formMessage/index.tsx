import './style.scss'

interface props{
    isActive: boolean,
    type: 'error' | 'info'
    text: string
}

const FormMessage = ({isActive, text, type}: props) => {

    return(
        <div className={`form-message ${type} ${isActive? 'active' : ''}`}>
            {
                type == 'error'?
                <svg focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path fillRule="evenodd" clipRule="evenodd" d="M12 20a8 8 0 100-16 8 8 0 000 16zm2.293-4.293L12 13.414l-2.293 2.293-1.414-1.414L10.586 12 8.293 9.707l1.414-1.414L12 10.586l2.293-2.293 1.414 1.414L13.414 12l2.293 2.293-1.414 1.414z"></path></svg>
                :
                <svg focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 7h2v2h-2zm0 4h2v6h-2zm1-9C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z"></path></svg>
            }
            <p>{text}</p>
        </div>
    )
}
export default FormMessage