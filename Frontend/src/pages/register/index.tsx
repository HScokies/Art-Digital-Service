import { Validator } from 'src/services'
import './style.scss'
import Logo from 'images/logo.webp'
import { useNavigate, useParams } from 'react-router-dom'
import { useContext, useState } from 'react'
import { Button, Combobox, Input } from 'src/components'
import { Option } from 'src/components/combobox'

const RegisterPage = () => {
    const { Email } = useParams()
    const navigate = useNavigate()
    const [hasErrors, setHasErrors] = useState(true);

    interface IForm {
        email: HTMLInputElement,
        password: HTMLInputElement
    }
    const handleChange = () => {
        const form: IForm = (document.querySelector("#register-form") as unknown) as IForm
        if (Validator.validateEmail(form.email.value) || Validator.validatePassword(form.password.value)) {
            setHasErrors(true)
        } else setHasErrors(false)
    }


    return (
        <div className="authpage">
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <h1 className='authpage_modal-title'>Зарегистрироваться</h1>
                <form id='register-form' onSubmit={() => navigate('/form')}>
                    <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange} defaultValue={Email} />
                    <Combobox name='typeId' label='Тип учетной записи' options={[]} />
                    <Input label='Пароль' type='password' name='password' required={true} validator={Validator.validatePassword} onChange={handleChange} maxlength={24} />
                    <Button isActive={!hasErrors}>Продолжить</Button>
                </form>
            </div>
        </div>
    )
}

export default RegisterPage