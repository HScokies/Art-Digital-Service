import { useState } from 'react'
import './style.scss'
import Icons from 'images/icons.svg'

interface props {
    name: string,
    label: string,
    accept?: string[]
    required?: true | undefined
}
const FileInput = ({ name, label, required, accept }: props) => {
    const [filename, setFileName] = useState<undefined | string>()
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const fileName = e.target.value.split(/(\\|\/)/g).pop();
        if (fileName)
            setFileName(fileName)
    }
    const openFileDialog = () => {
        document.getElementById(name)?.click()
    }
    return (
        <div className="fileinput">
            <div className={`fileinput_wrapper`} onClick={() => openFileDialog()}>
                <input className='fileinput_wrapper-field' type='file' name={name} id={name} required={required} accept={accept?.join(', ')} onChange={(e) => handleChange(e)} />
                <div className='fileinput_wrapper_text'>
                    <label className={`fileinput_wrapper_text-label ${filename? 'active' : ''}`} htmlFor={name}>{label}</label>
                    <span className='fileinput_wrapper_text-file'>{filename}</span>
                </div>
                <svg className='fileinput_wrapper-icon'>
                    <use xlinkHref={Icons + "#upload"} />
                </svg>
            </div>
        </div>
    );
}

export default FileInput