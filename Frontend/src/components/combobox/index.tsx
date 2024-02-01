import './style.scss'
import { useEffect, useState } from 'react';

export interface Option {
    value: any,
    label: string
}



interface props {
    name: string,
    label: string,    
    defaultValue?: any,
    options: Option[],
    changeHandler?: (e: React.ChangeEvent<HTMLSelectElement>) => void
}

const Combobox = ({ name, label, defaultValue, options, changeHandler = () => { } }: props) => {
    const [id] = useState(name + Date.now().toString(36))
    const [value, setValue] = useState(defaultValue)
    useEffect(() => {
        setValue(defaultValue)
    }, [defaultValue])

    const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        changeHandler(e)
        setValue(e.target.value)
    }
    return (
        <div className="combobox">  
            <div className='combobox_wrapper'>
                <label className='combobox_wrapper-label' htmlFor={id}>{label}</label>
                <select id={id} name={name} value={value} onChange={(e) => handleChange(e)} className='combobox_wrapper-input'>
                    {
                        options.map((data) => (
                            <option key={data.value} value={data.value}>{data.label}</option>
                        ))
                    }
                </select>
            </div>
        </div>
    );
}
export default Combobox