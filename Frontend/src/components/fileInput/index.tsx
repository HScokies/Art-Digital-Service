import { useEffect, useState } from 'react'
import './style.scss'
import Icons from 'images/icons.svg'

interface props {
    initialFileName?: string,
    downloadLink?: string,
    name: string,
    label: string,
    accept?: string[]
    required?: true | undefined,
    changeHandler?: (e: React.ChangeEvent<HTMLInputElement>) => void
}
const FileInput = ({ initialFileName, downloadLink, name, label, required, accept, changeHandler }: props) => {
    const [filename, setFileName] = useState<string>()
    const [id] = useState(name + '-' + Date.now().toString(36))
    const [showDownload, setShowDownload] = useState(downloadLink != undefined)
    useEffect(() => {
        setFileName(initialFileName)
    }, [initialFileName])

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const fileName = e.target.value.split(/(\\|\/)/g).pop();
        if (fileName != undefined) {
            setFileName(fileName)
            setShowDownload(false)
        }
        if (changeHandler) changeHandler(e)
    }

    const openFileDialog = () => {
        document.getElementById(id)?.click()
    }
    return (
        <div className="fileinput">
            <div className={`fileinput_wrapper`} onClick={() => openFileDialog()}>
                <input className='fileinput_wrapper-field' type='file' name={name} id={id} required={required} accept={accept?.join(', ')} onChange={(e) => handleChange(e)} />
                <div className='fileinput_wrapper_text'>
                    <label className={`fileinput_wrapper_text-label ${filename != undefined ? 'active' : ''}`} htmlFor={name}>{label}</label>
                    <span className='fileinput_wrapper_text-file'>{filename}</span>
                </div>
                <svg className='fileinput_wrapper-icon fileinput_wrapper-icon-upload'>
                    <use xlinkHref={Icons + "#upload"} href={Icons + "#upload"} />
                </svg>
            </div>
            {
                showDownload &&
                <a href={downloadLink} target='_blank'>
                    <svg className='fileinput_wrapper-icon download'>
                        <use xlinkHref={Icons + "#download"} href={Icons + "#download"} />
                    </svg>
                </a>
            }
        </div>
    );
}

export default FileInput