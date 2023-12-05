import './style.scss'
import { useParams } from 'react-router-dom';
import Logo from 'images/logo.webp'
import { Button, Input } from 'src/components';
import { Validator } from 'src/services';
import { useState } from 'react';

interface IForm{
    email: HTMLInputElement,
    password: HTMLInputElement
}

const LoginPage = () => {
    const { Email } = useParams()
   console.log("param",Email) 
    const [hasErrors, setHasErrors] = useState(true);

    const handleChange = () => {
        const form: IForm = (document.querySelector("#login-form") as unknown) as IForm
        if (Validator.validateEmail(form.email.value) || Validator.validatePassword(form.password.value)){
            setHasErrors(true)
        } else setHasErrors(false)
    }

    return (
        <div className="authpage">
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <h1 className='authpage_modal-title'>Войти в учетную запись</h1>
                <form id='login-form'>
                <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange}  defaultValue={Email} />
                <Input label='Пароль' type='password' name='password' required={true} validator={Validator.validatePassword} onChange={handleChange} maxlength={24} />
                </form>
                <Button isActive={!hasErrors}>Продолжить</Button>
            </div>
        </div>
    );
}

export default LoginPage