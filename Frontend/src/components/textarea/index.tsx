import { useEffect, useRef, useState } from 'react'
import './style.scss'

interface props {
    label: string,
    name: string,
    required?: true | undefined,
    onChange?: (e: React.ChangeEvent<HTMLTextAreaElement>) => void,
    onKeyUp?: (e: React.KeyboardEvent<HTMLTextAreaElement>) => void,
    onBlur?: (text?: string) => void
    defaultValue?: any,
    maxlength?: number,
}

const TextArea = ({ label, name, required = undefined, onChange, onKeyUp, onBlur, defaultValue, maxlength }: props) => {
    const [id] = useState(name + '-' + (Date.now() * Math.random()).toString(36))
    const [value, setValue] = useState<string>(defaultValue)
    const [active, setActive] = useState(defaultValue ? true : false)
    const [height, setHeight] = useState(0)
    const thisElement = useRef(null)

    const EnableField = async () => {
        await setActive(true)
        const element = thisElement.current
        if (element) (element as HTMLElement).focus()
    }

    const handleBlur = async () => {
        if (onBlur)
            onBlur(value)
        
        const field = (thisElement.current as unknown) as HTMLTextAreaElement
        if (field?.value.length < 1) {
            return setActive(false)
        }
    }
    const handleChange = async (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setValue(e.target.value)
        toggleHeight()
    }
    const toggleHeight = async (height?: number) => {
        const element = (thisElement.current as unknown) as HTMLTextAreaElement
        await setHeight(0)
        setHeight(height || element.scrollHeight)
    }

    useEffect(() => {
        setValue(defaultValue)
        toggleHeight() // no idea how 2 fix this thing 
    }, [defaultValue])

    return (
        <div className="textarea">
            <div className='textarea_wrapper' onClick={() => EnableField()}>
                <textarea rows={2} ref={thisElement} style={{ height: height }} value={value} placeholder=' ' maxLength={maxlength} onChange={(e) => { if (onChange) onChange(e); handleChange(e) }} onKeyUp={(e) => { if (onKeyUp) onKeyUp(e) }} onFocus={() => EnableField()} onBlur={() => handleBlur()} className={`textarea_wrapper-field${active ? ' active' : ''}`} id={id} name={name} required={required}>{value}</textarea>
                <label className={`textarea_wrapper-label${active ? ' active' : ''}`} htmlFor={name}>{label}</label>
            </div>
        </div>
    );
}

export default TextArea