import { useState } from 'react'
import './style.scss'

interface props {
    children: any,
    name: string,
    defaultValue?: boolean,
}

const Checkbox = ({ children, name, defaultValue = false }: props) => {
    const [checked, setChecked] = useState(defaultValue)

    return (
        <div className='checkbox'>
            <label className='checkbox-label' htmlFor={name}>
                {
                    children
                }
            </label>
            <input onChange={() => setChecked(!checked)} className='checkbox-input' type='checkbox' id={name} name={name} checked={checked} />
            <span onClick={() => setChecked(!checked)} className={'checkbox-input_box'}>
                {
                    checked ?
                        <svg viewBox="0 0 32 22" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <path d="M27.761 0L11.4565 13.9061L4.56127 7.14104L0 11.0706L11.1396 22L32 4.22447L27.761 0Z" fill="#6B74B2" />
                        </svg> :
                        null
                }
            </span>
        </div>
    )
}
export default Checkbox