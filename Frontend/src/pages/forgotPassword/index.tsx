import './style.scss'
import { Link, useNavigate, useParams } from 'react-router-dom';
import Logo from 'images/logo.webp'
import { Button, Input, FormMessage } from 'src/components';
import { API, Validator } from 'src/services';
import { useState } from 'react';
import './style.scss'
import { Pages } from 'src/enums';

interface IFormError {
    isActive: boolean,
    text: string
}

const ForgotPasswordPage = () => {
    const { Email } = useParams()
    const [hasErrors, setHasErrors] = useState<boolean>(Validator.validateEmail(Email as string).length > 0);
    const [showFormMessage, setShowFormMessage] = useState(false)
    const [email, setEmail] = useState(Email)
    const [formError, setFormError] = useState<IFormError>({
        isActive: false,
        text: ""
    });
    const navigate = useNavigate()

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (Validator.validateEmail(e.target.value)){
            setHasErrors(true)
        }
        else setHasErrors(false)
        setEmail(email)

    }

    const clearFormError = () => {
        setFormError({
            isActive: false,
            text: ""
        });
    }

    const handleSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        
        const response = await API.forgotPassword(new FormData(e.target as HTMLFormElement))
        if (response.status != 204){
            setShowFormMessage(false)
            const data = response.data;
            return setFormError({
                isActive: true,
                text: data.title
            });

        }
        clearFormError()
        setShowFormMessage(true)
        setTimeout(() => {
            navigate(Pages.auth)
        }, 3000)
    }

    return (
        <div className="authpage">
            <div className="authpage_modal">
                <Link to={Pages.auth}><img loading="lazy" alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' /></Link>
                <FormMessage type='error' text={formError.text} isActive={formError.isActive} />
                <FormMessage type='info' text={"Проверьте свою электронную почту на наличие ссылки для сброса пароля. Если оно не появится в течение нескольких минут, проверьте папку «Спам»."} isActive={showFormMessage}/>                
                <h1 className='authpage_modal-title'>Забыли пароль?</h1>
                <form id='forgot-form' onSubmit={(e) => handleSubmit(e)}>
                    <span className={`authpage_modal-tip`}>Введите адрес электронной почты вашей учетной записи, и мы вышлем вам ссылку для сброса пароля.</span>
                    <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} defaultValue={Email} onChange={handleChange} />
                    <Button isActive={!hasErrors}>Отправить</Button>
                </form>                
            </div>
        </div>
    )
}

export default ForgotPasswordPage