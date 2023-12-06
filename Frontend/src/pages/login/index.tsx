import './style.scss'
import { useParams } from 'react-router-dom';
import Logo from 'images/logo.webp'
import { Button, Input } from 'src/components';
import { Validator } from 'src/services';
import { useState } from 'react';
import { FormError } from 'src/components/index';

interface IForm{
    email: HTMLInputElement,
    password: HTMLInputElement
}

interface IFormError{
    isActive: boolean,
    text: string
}

const LoginPage = () => {
    const { Email } = useParams()
    const [hasErrors, setHasErrors] = useState(true);

    const [formError, setFormError] = useState<IFormError>({
        isActive: false,
        text: ""
    });

    const handleChange = () => {
        const form: IForm = (document.querySelector("#login-form") as unknown) as IForm
        if (Validator.validateEmail(form.email.value) || Validator.validatePassword(form.password.value)){
            setHasErrors(true)
        } else setHasErrors(false)
    }

    const handleSubmit = () => {
        setFormError({
            isActive: true,
            text: "Данные, использованные вами для входа, недействительны"
        });
    }

    return (
        <div className="authpage">
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <FormError text={formError.text} isActive={formError.isActive}/>
                <h1 className='authpage_modal-title'>Войти в учетную запись</h1>
                <form id='login-form'>
                <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange}  defaultValue={Email} />
                <Input label='Пароль' type='password' name='password' required={true} validator={Validator.validatePassword} onChange={handleChange} maxlength={24} />
                </form>
                <Button isActive={!hasErrors} clickHandler={handleSubmit}>Продолжить</Button>
            </div>
        </div>
    );
}

export default LoginPage