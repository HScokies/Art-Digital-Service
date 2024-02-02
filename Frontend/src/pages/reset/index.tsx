import './style.scss'
import { Link, useNavigate, useParams } from 'react-router-dom';
import Logo from 'images/logo.webp'
import { Button, Input, FormMessage } from 'src/components';
import { API, Validator } from 'src/services';
import { useEffect, useState } from 'react';
import { Pages } from 'src/enums';
import { error } from 'console';

interface IForm{
    password: HTMLInputElement,
}

interface IFormError{
    isActive: boolean,
    text: string
}

const PasswordResetPage = () => {
    const { Token } = useParams()
    const navigate = useNavigate()

    const [hasErrors, setHasErrors] = useState(true);
    const [formError, setFormError] = useState<IFormError>({
        isActive: false,
        text: ""
    });
    

    const handleChange = () => {
        const form: IForm = (document.querySelector("#login-form") as unknown) as IForm
        if (Validator.validatePassword(form.password.value)){
            setHasErrors(true)
        } else setHasErrors(false)
    }

    const setError = (error: string) => {
        setFormError({
            isActive: true,
            text: error
        })
    }

    const handleSubmit = async(e : React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const response = await API.resetPassword(Token!, new FormData(e.target as HTMLFormElement))
        if (response.status != 204){
            const data = response.data;
            return setError(data.title)
        }
        navigate(Pages.auth)
    }

    useEffect(() => {
        if (!Token) navigate(Pages.auth)
    }, [Token])

    return(
        <div className="authpage">
        <div className="authpage_modal">
            <Link to={Pages.auth}><img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' /></Link>
            <FormMessage type='error' text={formError.text} isActive={formError.isActive}/>
            <h1 className='authpage_modal-title'>Обновление пароля</h1>
            <form id='login-form' onSubmit={(e) => handleSubmit(e)}>
            <Input label='Новый пароль' type='password' name='password' required={true} validator={Validator.validatePassword} onChange={handleChange} maxlength={24} />
            <Button isActive={!hasErrors}>Продолжить</Button>
            </form>            
        </div>
    </div>
    )
}

export default PasswordResetPage