import './style.scss'
import Logo from 'images/logo.webp'
import { useState } from 'react'
import { Button, Combobox, Input } from 'src/components'
import { Option } from 'src/components/combobox'

const PersonalDataPage = () => {
    const [cases, setCases] = useState<Option[]>([
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
    ]) 
    return (
        <div className="authpage">
            <div className="authpage_modal">
                <img alt='logo' src={Logo} draggable={false} className='authpage_modal-logo' />
                <h1 className='authpage_modal-title'>Зарегистрироваться</h1>
                <form id='login-form'>
                    <Combobox label='Направление' options={cases} />
                    <Input datalist={"countries"} label='Город участника' type='text' name='city' required={true} />
                    <datalist id='countries'>
                        <option value="Челябинск"></option>
                        <option value="Копейск"></option>
                        <option value="Южноуральск"></option>
                        <option value="Миасс"></option>
                        <option value="Златоуст"></option>
                        <option value="Сосновский муниципальный район"></option>
                    </datalist>
                </form>
                <Button isActive={true}>Продолжить</Button>
            </div>
        </div>
    )
}
export default PersonalDataPage