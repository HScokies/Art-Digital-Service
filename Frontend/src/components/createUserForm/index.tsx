import { useEffect, useState } from "react"
import { Option } from "src/components/combobox"
import { ICity, ICreateForm, IUserType } from "src/interfaces"
import { API, PhoneChange, Validator } from "src/services"
import { Combobox, Input, FileInput } from "components/index"


const CreateUserForm = ({formId}: ICreateForm) => {
    const [isAdult, setIsAdult] = useState(true)
    const [cases, setCases] = useState<Option[]>([])
    const [userTypes, setUserTypes] = useState<IUserType[]>()
    const [userTypeOptions, setUserTypeOptions] = useState<Option[]>([])
    const [cities, setCities] = useState<ICity[]>()

    useEffect(() => {
        const getCases = async() => {
            const data = (await API.getCases()).data
            let casesAsOptions: Option[] = []
            for (let _case of data) {
                casesAsOptions.push({
                    value: _case.id,
                    label: _case.name
                })
            }
            setCases(casesAsOptions)
        }
        const getUserTypes = async() => {
            const data = (await API.getParticipantTypes()).data
            setUserTypes(data)
            let userTypeOptions: Option[] = []
            for (let userType of data) {
                userTypeOptions.push({
                    value: userType.id,
                    label: userType.name
                })
            }
            setUserTypeOptions(userTypeOptions)
            setIsAdult(data[0].isAdult);
        }     
        const getCities = async() => {
            const response = await API.getCities()
            if (response.status != 200) return;
            const data = response.data
            setCities(data)
        }   

        getCases()
        getUserTypes()
        getCities()
    }, [])

    const toggleUserType = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (!userTypes) return
        const newID = +e.target.value
        const userType = userTypes.find(ut => ut.id == newID)
        if (!userType) return
        setIsAdult(userType?.isAdult)
    }

    const handleSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        await API.createUser(new FormData(e.target as HTMLFormElement))
    }
    
    return (       
        <form id={formId} onSubmit={(e) => handleSubmit(e)}>
            <Input label='Адрес электронной почты' type='email' name='email'  validator={Validator.validateEmail} required />
            <Input label='Пароль' type='password' name='password'  validator={Validator.validatePassword} required />
            <Combobox name="typeId" label='Тип учетной записи' options={userTypeOptions} changeHandler={toggleUserType} />
            {!isAdult && <Input label='Полное имя родителя' type='text' name='parentName' required />}
            <Input onChange={PhoneChange} defaultValue="+7 " maxlength={13} label='Телефон' type='tel' name='phone'  validator={Validator.validatePhoneNumber} required />
            <Input label='Фамилия участника' type='text' name='lastName'  maxlength={17} required />
            <Input label='Имя участника' type='text' name='firstName'  maxlength={17} required />
            <Input label='Отчество участника' type='text' name='patronymic'  maxlength={20} required />
            <Input datalist="Cities" label='Город участника' type='text' name='city' required />
            <datalist id="Cities">
                {
                    cities?.map((e) => 
                    <option value={e.name} key={e.id}></option>
                    )
                }
            </datalist>
            <Input label='Учебное заведение' type='text' name='institution' required/>
            {
                isAdult ?
                    <>
                        <Input label='Курс' type='number' name='grade' validator={Validator.validateGradeStudent} min={1} max={11} required />
                        <Input label='Специальность' type='text' name='speciality' required />
                    </> :
                    <Input label='Класс' type='number' name='grade' validator={Validator.validateGradeSchool} min={1} max={11} required />
            }
            <Combobox name="caseId" label='Направление' options={cases} />
            <FileInput label='Согласие на обработку персональных данных' name='consent' accept={['.pdf']} />
            <FileInput label='Выполненное задание' name='solution' accept={['.pdf']} />
        </form>
    )

}
export default CreateUserForm