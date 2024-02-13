import { useEffect, useState } from 'react'
import './style.scss'
import Icons from 'images/icons.svg'

interface props {
    children: any,
    name: string,
    defaultValue?: boolean,
    checkedChanged?: ()=>void
}

const Checkbox = ({ children, name, defaultValue = false, checkedChanged }: props) => {
    const [checked, setChecked] = useState(defaultValue)
    useEffect(() => {
        if (checkedChanged)
            checkedChanged()
    },[checked])

    return (
        <div className='checkbox' onClick={() => {if (checkedChanged) checkedChanged()}}>
            <label className='checkbox-label' htmlFor={name}>
                {
                    children
                }
            </label>
            <input onChange={() => setChecked(!checked)} className='checkbox-input' type='checkbox' id={name} name={name} checked={checked} />
            <span onClick={() => setChecked(!checked)} className={'checkbox-input_box'}>
                {
                    checked ?
                        <svg>
                            <use xlinkHref={Icons+'#check'} href={Icons+'#check'} />
                        </svg> :
                        null
                }
            </span>
        </div>
    )
}
export default Checkbox