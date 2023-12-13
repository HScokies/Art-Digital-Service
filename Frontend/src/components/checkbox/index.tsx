import { useState } from 'react'
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

    return (
        <div className='checkbox'>
            <label className='checkbox-label' htmlFor={name}>
                {
                    children
                }
            </label>
            <input onChange={() => setChecked(!checked)} className='checkbox-input' type='checkbox' id={name} name={name} checked={checked} />
            <span onClick={async() => {await setChecked(!checked); if (checkedChanged) checkedChanged()}} className={'checkbox-input_box'}>
                {
                    checked ?
                        <svg>
                            <use xlinkHref={Icons+'#check'} />
                        </svg> :
                        null
                }
            </span>
        </div>
    )
}
export default Checkbox