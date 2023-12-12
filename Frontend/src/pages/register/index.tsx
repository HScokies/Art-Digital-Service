import { Validator } from 'src/services'
import './style.scss'
import Logo from 'images/logo.webp'
import { useParams } from 'react-router-dom'
import { useState } from 'react'
import { Button, Combobox, Input } from 'src/components'
import { Option } from 'src/components/combobox'

const RegisterPage = () => {
    const { Email } = useParams()
    const [hasErrors, setHasErrors] = useState(true);
    const [userTypes, setUserTypes] = useState<Option[]>([
        {
            value: 0,
            label: "Студент СПО"
        },
        {
            value: 1,
            label: "Студент ВО"
        },
        {
            value: 2,
            label: "Школьник"
        },
    ])
    const handleChange = () => {

    }

    return (
        <div className="authpage">
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <h1 className='authpage_modal-title'>Зарегистрироваться</h1>
                <form id='login-form'>
                    <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange} defaultValue={Email} />
                    <Combobox label='Тип учетной записи' options={userTypes} />
                    <Input label='Пароль' type='password' name='password' required={true} validator={Validator.validatePassword} onChange={handleChange} maxlength={24} />
                </form>
                <Button isActive={!hasErrors}>Продолжить</Button>
            </div>
        </div>
    )
}

export default RegisterPage