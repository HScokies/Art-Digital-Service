import { Button, Input } from 'src/components'
import './style.scss'
import Logo from 'images/logo.webp'
import { API, Validator } from 'src/services'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Pages } from 'src/enums'

const AccessPage = () => {
    const navigate = useNavigate()
    const [hasErrors, setHasErrors] = useState(true);
    const [email, setEmail] = useState("");

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value)
        if (Validator.validateEmail(e.target.value)) {
            setHasErrors(true)
        }
        else setHasErrors(false)

    }

    const handleSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const response = await API.emailExists(email)
        if (response.status != 200) return;

        const isExists = response.data as boolean;
        isExists? navigate(Pages.login + email) : navigate(Pages.register + email)
    }

    return (
        <div className='authpage'>
            <div className="authpage_modal">
                <img loading="lazy" alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <h1 className='authpage_modal-title'>Войдите или зарегистрируйтесь</h1>
                <form id='access-form' onSubmit={async(e) => await handleSubmit(e)}>
                    <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange} defaultValue={email} />
                    <Button type='submit' isActive={!hasErrors}>Продолжить</Button>
                </form>
            </div>
        </div>
    )
}
export default AccessPage