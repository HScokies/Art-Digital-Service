import { Button, Input } from 'src/components'
import './style.scss'
import Logo from 'images/logo.webp'
import { Validator } from 'src/services'
import { useState } from 'react'

const AccessPage = () => {
    const [hasErrors, setHasErrors] = useState(false);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (!Validator.validateEmail(e.target.value)){
            setHasErrors(false)
        }
        else setHasErrors(true)
    }

    return (
        <div className='accesspage'>
            <div className="accesspage_modal">
                <img alt='logo' src={Logo} draggable={false} className='accesspage_modal-logo'/>
                <h1 className='accesspage_modal-title'>Войдите или зарегистрируйтесь</h1>
                <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange}/>
                <Button isActive={!hasErrors}>Продолжить</Button>
            </div>
        </div>
    )
}
export default AccessPage