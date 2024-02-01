import './style.scss'
import Logo from 'images/logo.webp'
import { useEffect, useState } from 'react'
import { Button, Combobox, Input, Checkbox } from 'src/components'
import { Option } from 'src/components/combobox'
import { ICase, ICity } from 'src/interfaces'
import { API, Validator } from 'src/services'

const PersonalDataPage = () => {
    const [hasErrors, setHasErrors] = useState(true);

    const [cases, setCases] = useState<Option[]>([])
    const [cities, setCities] = useState<ICity[]>([])

    const dbdata = {
        isAdult: true,
        cases: [
            {
                value: 0,
                label: "Графический дизайн"
            },
            {
                value: 1,
                label: "Дизайн одежды"
            },
            {
                value: 2,
                label: "Дизайн интерьера"
            },
            {
                value: 3,
                label: "3D моделирование для компьютерных игр"
            },
            {
                value: 4,
                label: "Веб-разработка"
            },
            {
                value: 5,
                label: "Разработка компьютерных игр"
            },
            {
                value: 6,
                label: "Туризм"
            },
            {
                value: 7,
                label: "Ресторанное дело"
            },
            {
                value: 8,
                label: "Интернет-маркетинг"
            },
            {
                value: 9,
                label: "Веб-дизайн"
            },
        ],
        cities: [
            "Челябинск",
            "Копейск",
            "Южноуральск",
            "Миасс",
            "Златоуст",
            "Сосновский муниципальный район"
        ]
    }

    useEffect(() => {
        const getCases = async() => {
            const response = await API.getCases()
            if (response.status != 200) return
            const data = response.data as ICase[];
            const caseOptions: Option[] = []
            for (let _case of data){
                caseOptions.push({
                    label: _case.name,
                    value: _case.id
                })
            }
            setCases(caseOptions)
        }
        const getCities  =async () => {
            const response = await API.getCities()
            if (response.status != 200) return
            setCities(response.data)
        }

        getCases()
        getCities()
    }, [])

    const validateFields = () => {
        setHasErrors(true)
        const form = document.querySelector("#personaldata-form") as HTMLFormElement
        const checkboxs = form.getElementsByClassName("checkbox-input")
        for (let i=0; i<checkboxs.length; i++){
            const checkbox = checkboxs[i] as HTMLInputElement;
            if (!checkbox.checked){                
                return;
            }
        }

        const inputs = form.getElementsByClassName("input_wrapper-field")        
        for (let i=0; i<inputs.length; i++){
            const input = inputs[i] as HTMLInputElement;

            if (input.value.length < 1) return;
            if (input.type == "tel" && input.value.length != 13) return
        }
        setHasErrors(false)
    }

    const validateGrade = (e: React.KeyboardEvent<HTMLInputElement>) => {
        const numberInput = e.target as HTMLInputElement;
        if (!numberInput.value.match(/^-?\d+$/)){
            numberInput.value = ""   
        }
        else numberInput.value = (+numberInput.value).toString()
        validateFields()
    }

    const phoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const phone = e.target.value
        let startAdding = false
        let newPhone = "+7 "

        for (let i=0; i<phone.length; i+=1){
            if (phone[i] == '+' && phone[i+1] == '7' && phone[i+2] == ' '){
                startAdding = true;
                i += 2;
                continue;
            }
            
            if (!startAdding)
                continue;
            
            if (!isNaN(+phone[i]) && phone[i] != ' ')
                newPhone+=phone[i];
        }
        e.target.value = newPhone;  
        validateFields()
    }

    return (
        <div className="authpage">
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <h1 className='authpage_modal-title'>Персональные данные</h1>
                <form id='personaldata-form' onSubmit={() => console.debug("submit")}>
                    {
                        dbdata.isAdult ?
                            null :
                            <Input onChange={validateFields} label='Полное имя родителя' type='text' name='parentName' required={true} />
                    }
                    <Input defaultValue={"+7 "} onChange={phoneChange} maxlength={13} label='Телефон' type='tel' name='phone' required={true} validator={Validator.validatePhoneNumber} />
                    <Input onChange={validateFields} label='Фамилия участника' type='text' name='lastName' required={true} maxlength={20} />
                    <Input onChange={validateFields} label='Имя участника' type='text' name='firstName' required={true} maxlength={20} />
                    <Input onChange={validateFields} label='Отчество участника' type='text' name='patronymic' required={true} maxlength={20} />
                    <Input onChange={validateFields} datalist="Cities" label='Город участника' type='text' name='city' required={true} />
                    <datalist id='Cities'>
                        {
                            cities?.map((e) => (
                                <option key={e.id} value={e.name}></option>
                            ))
                        }
                    </datalist>
                    <Input onChange={validateFields} label='Учебное заведение' type='text' name='institution' required={true} />
                    {
                        dbdata.isAdult ?
                            <>
                                <Input onKeyUp={validateGrade} label='Курс' type='number' name='grade' required={true} validator={Validator.validateGradeStudent} min={1} max={11} />
                                <Input onChange={validateFields} label='Специальность' type='text' name='speciality' required={true} />
                            </> :
                            <>
                                <Input onKeyUp={validateGrade} label='Класс' type='number' name='grade' required={true} validator={Validator.validateGradeSchool} min={1} max={11} />
                            </>
                    }
                    <Combobox name='case' label='Направление' options={cases} />
                    <Checkbox checkedChanged={validateFields} name='acceptedPrivacyPolicy'>
                        Ознакомлен с <a target='_blank' href='https://disk.yandex.ru/i/FrvixFf4IKNtwg'>политикой&nbsp;конфиденциальности</a>
                    </Checkbox>
                    <Checkbox checkedChanged={validateFields} name='acceptedProcessing'>
                        Даю согласие на обработку <a target='_blank' href='https://disk.yandex.ru/i/brJofU0LLzyn6w'>персональных&nbsp;данных</a>
                    </Checkbox>
                    <Button isActive={!hasErrors}>Продолжить</Button>
                </form>
            </div>
        </div>
    )
}
export default PersonalDataPage