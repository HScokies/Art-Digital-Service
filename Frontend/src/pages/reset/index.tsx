import './style.scss'
import { useParams } from 'react-router-dom';
import Logo from 'images/logo.webp'
import { Button, Input, FormMessage } from 'src/components';
import { Validator } from 'src/services';
import { useState } from 'react';

interface IForm{
    password: HTMLInputElement,
}

interface IFormError{
    isActive: boolean,
    text: string
}

const PasswordResetPage = () => {
    const { Token } = useParams()

    const [formError, setFormError] = useState<IFormError>({
        isActive: false,
        text: ""
    });
    const [hasErrors, setHasErrors] = useState(true);

    const handleChange = () => {
        const form: IForm = (document.querySelector("#login-form") as unknown) as IForm
        if (Validator.validatePassword(form.password.value)){
            setHasErrors(true)
        } else setHasErrors(false)
    }

    const handleSubmit = () => {
        setFormError({
            isActive: true,
            text: "Время действия ссылки истекло, запросите новую"
        });
    }

    return(
        <div className="authpage">
        <div className="authpage_modal">
            <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
            <FormMessage type='error' text={formError.text} isActive={formError.isActive}/>
            <h1 className='authpage_modal-title'>Обновление пароля</h1>
            <form id='login-form'>
            <Input label='Новый пароль' type='password' name='password' required={true} validator={Validator.validatePassword} onChange={handleChange} maxlength={24} />
            <Button isActive={!hasErrors} clickHandler={handleSubmit}>Продолжить</Button>
            </form>            
        </div>
    </div>
    )
}

export default PasswordResetPage