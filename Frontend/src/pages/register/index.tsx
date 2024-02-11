import { API, Validator } from 'src/services'
import './style.scss'
import Logo from 'images/logo.webp'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { useEffect, useState } from 'react'
import { Button, Combobox, FormMessage, Input } from 'src/components'
import { Option } from 'src/components/combobox'
import { IUserType } from 'src/interfaces'
import { UseAuth } from 'src/hooks/useAuth'
import { Pages, UserTypes } from 'src/enums'

interface IFormError {
    isActive: boolean,
    text: string
}

const RegisterPage = () => {
    const { Email } = useParams()
    const { setUserType } = UseAuth();

    const navigate = useNavigate()

    const [hasErrors, setHasErrors] = useState(true);
    const [options, setOptions] = useState<Option[]>([])
    const [formError, setFormError] = useState<IFormError>({
        isActive: false,
        text: ""
    });

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

    const handleSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        const UserData = new FormData(e.target as HTMLFormElement)
        
        const response = await API.register(UserData)
        const data = response.data;
        if (response.status != 201){
            return setFormError({isActive: true, text: data.title})
        }
        setUserType(UserTypes.newUser);
        navigate("/form")
        
    }

    useEffect(() => {
        const getTypes = async() => {
            const response = await API.getParticipantTypes();
            if (response.status != 200) return;
            
            const types = response.data as IUserType[];            
            setOptions(types.map(type => ({value: type.id, label: type.name})))
        }
        getTypes();
    }, [])


    return (
        <div className="authpage">
            <div className="authpage_modal">
                <Link to={Pages.auth}><img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' /></Link>
                <FormMessage type='error' text={formError.text} isActive={formError.isActive} />
                <h1 className='authpage_modal-title'>Зарегистрироваться</h1>
                <form id='register-form' onSubmit={(e) => handleSubmit(e)}>
                    <Input label='Адрес электронной почты' readonly type='email' name='email' required={true} validator={Validator.validateEmail} onChange={handleChange} defaultValue={Email} />
                    <Combobox name='userType' label='Тип учетной записи' options={options} />
                    <Input label='Пароль' type='password' name='password' required={true} validator={Validator.validatePassword} onChange={handleChange} maxlength={24} />
                    <Button isActive={!hasErrors}>Продолжить</Button>
                </form>
            </div>
        </div>
    )
}

export default RegisterPage