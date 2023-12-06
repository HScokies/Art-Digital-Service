import { Button, Input } from 'src/components'
import './style.scss'
import Logo from 'images/logo.webp'
import { Validator } from 'src/services'
import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

const AccessPage = () => {
    const navigate = useNavigate()
    const [hasErrors, setHasErrors] = useState(true);
    const [email, setEmail] = useState("");

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value)
        if (Validator.validateEmail(e.target.value)){
            setHasErrors(true)
        }
        else setHasErrors(false)

    }

    const handleSubmit = () => {
        console.debug(email)
        console.debug("*api call*")
        if (email == "exists@email.com")
            navigate(`/login/${email}`)
        else navigate(`/register/${email}`)
    }

    return (
        <div className='authpage'>
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo'/>
                <h1 className='authpage_modal-title'>Войдите или зарегистрируйтесь</h1>
                <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange} defaultValue={email}/>
                <Button isActive={!hasErrors} clickHandler={handleSubmit}>Продолжить</Button>
            </div>
        </div>
    )
}
export default AccessPage