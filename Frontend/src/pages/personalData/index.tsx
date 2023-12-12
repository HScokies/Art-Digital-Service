import './style.scss'
import Logo from 'images/logo.webp'
import { useState } from 'react'
import { Button, Combobox, Input, Checkbox } from 'src/components'
import { Validator } from 'src/services'

const PersonalDataPage = () => {
    const [hasErrors, setHasErrors] = useState(true);

    const dbdata = {
        isAdult: false,
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

    const validateFields = () => {
    }

    const validateGrade = (e: React.KeyboardEvent<HTMLInputElement>) => {
        const numberInput = e.target as HTMLInputElement;
        if (!numberInput.value.match(/^-?\d+$/))
            numberInput.value = ""
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
                    <Input onChange={validateFields} label='Телефон' type='tel' name='phone' required={true} />
                    <Input onChange={validateFields} label='Фамилия участника' type='text' name='lastName' required={true} />
                    <Input onChange={validateFields} label='Имя участника' type='text' name='firstName' required={true} />
                    <Input onChange={validateFields} label='Отчество участника' type='text' name='middleName' required={true} />
                    <Input onChange={validateFields} datalist="Cities" label='Город участника' type='text' name='city' required={true} />
                    <datalist id='Cities'>
                        {
                            dbdata.cities.map((city, index) => (
                                <option key={index} value={city}></option>
                            ))
                        }
                    </datalist>
                    <Input onChange={validateFields} label='Учебное заведение' type='text' name='institution' required={true} />
                    {
                        dbdata.isAdult ?
                            <>
                                <Input onKeyUp={validateGrade} label='Курс' type='number' name='grade' required={true} validator={Validator.validateGradeStudent} min={1} max={10} />
                                <Input onChange={validateFields} label='Специальность' type='text' name='speciality' required={true} />
                            </> :
                            <>
                                <Input label='Класс' type='text' name='grade' required={true} validator={Validator.validateGradeSchool} min={0} max={10} />
                            </>
                    }
                    <Combobox label='Направление' options={dbdata.cases} />
                    <Checkbox name='acceptedPrivacyPolicy'>
                    Ознакомлен с <a target='_blank' href='https://disk.yandex.ru/i/FrvixFf4IKNtwg'>политикой&nbsp;конфиденциальности</a>
                    </Checkbox>
                    <Button isActive={!hasErrors}>Продолжить</Button>
                </form>
            </div>
        </div>
    )
}
export default PersonalDataPage