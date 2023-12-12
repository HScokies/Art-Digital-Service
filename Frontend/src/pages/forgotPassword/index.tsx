import './style.scss'
import { useParams } from 'react-router-dom';
import Logo from 'images/logo.webp'
import { Button, Input, FormMessage } from 'src/components';
import { Validator } from 'src/services';
import { useState } from 'react';
import './style.scss'

const ForgotPasswordPage = () => {
    const { Email } = useParams()
    const [hasErrors, setHasErrors] = useState<boolean>(Validator.validateEmail(Email as string).length > 0);
    const [showMessage, setShow] = useState(false)

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (Validator.validateEmail(e.target.value)){
            setHasErrors(true)
        }
        else setHasErrors(false)

    }

    const handleSubmit = () => {
        setShow(true)
    }

    return (
        <div className="authpage">
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <FormMessage type='info' text={"Проверьте свою электронную почту на наличие ссылки для сброса пароля. Если оно не появится в течение нескольких минут, проверьте папку «Спам»."} isActive={showMessage}/>                
                <h1 className='authpage_modal-title'>Забыли пароль?</h1>
                <form id='forgot-form'>
                    <span className={`authpage_modal-tip ${showMessage? 'disabled' : ''}`}>Введите адрес электронной почты вашей учетной записи, и мы вышлем вам ссылку для сброса пароля.</span>
                    <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} defaultValue={Email} onChange={handleChange} />
                    <Button isActive={!hasErrors} clickHandler={handleSubmit}>Отправить</Button>
                </form>                
            </div>
        </div>
    )
}

export default ForgotPasswordPage