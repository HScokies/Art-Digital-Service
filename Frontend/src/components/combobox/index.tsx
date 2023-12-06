import './style.scss'

export interface Option {
    value: any,
    label: string
}

interface props {
    label: string,
    options: Option[],
    changeHandler?: (e: React.ChangeEvent<HTMLSelectElement>) => void
}

const Combobox = ({ label, options, changeHandler = () => { } }: props) => {
    return (
        <div className="combobox">  
            <div className='combobox_wrapper'>
                <label className='combobox_wrapper-label'>{label}</label>
                <select onChange={(e) => changeHandler(e)} className='combobox_wrapper-input'>
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