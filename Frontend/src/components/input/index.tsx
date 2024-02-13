import { useEffect, useRef, useState } from 'react';
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
    max?: number,
    readonly?: true | undefined
}

const Input = ({ label, type, name, required = undefined, validator, onChange, onKeyUp, defaultValue = "", maxlength, datalist = "", min, max, readonly = undefined}: props) => {

    const [error, setError] = useState<string>()
    const [value, setValue] = useState<any>("")
    const [active, setActive] = useState(defaultValue ? true : false)
    const [id] = useState(name + '-' + Date.now().toString(36))
    const [showPassword, setShowPassword] = useState(false)
    const inputRef = useRef<HTMLInputElement>(null)

    const EnableField = async () => {
        await setActive(true)
        await setError(undefined)
        inputRef.current?.focus()
    }

    const handleBlur = async () => {
        if (value != undefined && type != "tel")
            await setValue(String(value).trim())
        const fieldValue  = inputRef.current?.value || '';
        if (fieldValue.length < 1) {
            if (required) {
                setError("Обязательно")
            }
            return setActive(false)
        }
        if (validator) {
            await setError(validator(fieldValue));
        }

    }

    const changeHandler = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (onChange) onChange(e);
        if (!readonly) setValue(e.target.value)
    }

    useEffect(() => {
        defaultValue === null? setValue("") : setValue(defaultValue)
    }, [defaultValue])
    return (
        <div className="input">
            <div className={`input_wrapper ${error ? 'error' : ''}`} onClick={() => EnableField()}>
                <input ref={inputRef} readOnly={readonly} placeholder=' ' list={datalist} maxLength={maxlength} value={value} onChange={(e) => changeHandler(e)} onKeyUp={(e) => { if (onKeyUp) onKeyUp(e) }} onFocus={() => EnableField()} onBlur={() => handleBlur()} className={`input_wrapper-field${active ? ' active' : ''}`} id={id} name={name} type={showPassword ? "text" : type} min={min} max={max} required={required} />
                <label className={`input_wrapper-label${active ? ' active' : ''}`} htmlFor={name}>{label}</label>
                {
                    type == "password" && value?.length > 0 ?
                        <svg className='input_wrapper-show'>
                            {
                                showPassword ?
                                    <use xlinkHref={Icons + "#hidePassword"} href={Icons + "#hidePassword"} onClick={() => setShowPassword(false)} /> :
                                    <use xlinkHref={Icons + "#showPassword"} href={Icons + "#showPassword"} onClick={() => setShowPassword(true)} />
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