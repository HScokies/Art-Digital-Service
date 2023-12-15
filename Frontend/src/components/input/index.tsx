import { useState } from 'react';
import './style.scss'
import Icons from 'images/icons.svg'

interface props {
    label: string,
    type: 'email' | 'number' | 'password' | 'tel' | 'text' | 'url',
    name: string,
    required?: true | undefined,
    validator?: (value: string) => string,
    onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void,
    onKeyUp?: (e: React.KeyboardEvent<HTMLInputElement>) => void,
    defaultValue?: any,
    maxlength?: number,
    datalist?: string,
    min?: number,
    max?: number
}

const Input = ({ label, type, name, required = undefined, validator, onChange, onKeyUp, defaultValue = "", maxlength, datalist = "", min, max }: props) => {

    const [error, setError] = useState<string>()
    const [value, setValue] = useState(defaultValue)
    const [active, setActive] = useState(defaultValue ? true : false)

    const [showPassword, setShowPassword] = useState(false)

    const EnableField = async () => {
        await setActive(true)
        await setError(undefined)
        document.getElementById(name)?.focus()
    }

    const handleBlur = async () => {
        if (type != "tel")
            await setValue(value.trim())
        const field = document.getElementById(name) as HTMLInputElement
        if (field?.value.length < 1) {
            if (required) {
                setError("Обязательно")
            }
            return setActive(false)
        }
        if (validator) {
            await setError(validator(field?.value));
        }

    }

    return (
        <div className="input">
            <div className={`input_wrapper ${error ? 'error' : ''}`} onClick={() => EnableField()}>
                <input placeholder='' list={datalist} maxLength={maxlength} value={value} onChange={(e) => { if (onChange) onChange(e); setValue(e.target.value) }} onKeyUp={(e) => { if (onKeyUp) onKeyUp(e) }} onFocus={() => EnableField()} onBlur={() => handleBlur()} className={`input_wrapper-field${active ? ' active' : ''}`} id={name} name={name} type={showPassword ? "text" : type} min={min} max={max} required={required} />
                <label className={`input_wrapper-label${active ? ' active' : ''}`} htmlFor={name}>{label}</label>
                {
                    type == "password" && value?.length > 0 ?
                        <svg className='input_wrapper-show'>
                            {
                                showPassword ?
                                    <use xlinkHref={Icons + "#hidePassword"} onClick={() => setShowPassword(false)} /> :
                                    <use xlinkHref={Icons + "#showPassword"} onClick={() => setShowPassword(true)} />
                            }
                        </svg>
                        : null
                }
            </div>

            <p className="input-error">
                {error}
            </p>
        </div>
    );
}
export default Input