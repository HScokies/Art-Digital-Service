import { useState } from 'react';
import './style.scss'

interface props {
    label: string,
    type: string,
    name: string,
    required: boolean,
    validator?: (value: string) => string,
    onChange?: (e:React.ChangeEvent<HTMLInputElement>) => void
}

const Input = ({ label, type, name, required = false, validator, onChange }: props) => {

    const [error, setError] = useState<string | undefined>()
    const [active, setActive] = useState(false)


    const EnableField = async () => {
        await setActive(true)
        await setError(undefined)
        document.getElementById(name)?.focus()
    }
    const handleBlur = async() => {
        const field = document.getElementById(name) as HTMLInputElement
        if (field?.value.length < 1) {
            if (required) {
                setError("Обязательно")
            }
            return setActive(false)
        }
        if (validator){
            await setError(validator(field?.value)); 
        }

    }

    return (
        <div className="input">
            <div className={`input_wrapper ${error ? 'error' : ''}`} onClick={() => EnableField()}>
                <input onChange={(e) => {if (onChange) onChange(e)}} onFocus={() => EnableField()} onBlur={() => handleBlur()} className={`input_wrapper-field${active ? ' active' : ''}`} id={name} name={name} type={type} />
                <label className={`input_wrapper-label${active ? ' active' : ''}`} htmlFor={name}>{label}</label>
            </div>

            <p className="input-error">
                {error}
            </p>
        </div>
    );
}
export default Input