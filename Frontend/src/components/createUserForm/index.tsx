import { useEffect, useState } from "react"
import { Option } from "src/components/combobox"
import { IUserType } from "src/interfaces"
import { API, PhoneChange, Validator } from "src/services"
import { Combobox, Input, FileInput } from "components/index"

const CreateUserForm = () => {
    const [isAdult, setIsAdult] = useState(true)
    const [cases, setCases] = useState<Option[]>([])
    const [userTypes, setUserTypes] = useState<IUserType[]>()
    const [userTypeOptions, setUserTypeOptions] = useState<Option[]>([])

    useEffect(() => {
        const getCases = () => {
            const data = API.getCases()
            let casesAsOptions: Option[] = []
            for (let _case of data) {
                casesAsOptions.push({
                    value: _case.id,
                    label: _case.name
                })
            }
            setCases(casesAsOptions)
        }
        const getUserTypes = () => {
            const data = API.getUserTypes()
            setUserTypes(data)
            let userTypeOptions: Option[] = []
            for (let userType of data) {
                userTypeOptions.push({
                    value: userType.id,
                    label: userType.name
                })
            }
            setUserTypeOptions(userTypeOptions)
        }

        getCases()
        getUserTypes()
    }, [])

    const toggleUserType = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (!userTypes) return
        const newID = +e.target.value
        const userType = userTypes.find(ut => ut.id == newID)
        if (!userType) return
        setIsAdult(userType?.isAdult)
    }

    return (

        <>
            <Input label='Адрес электронной почты' type='email' name='email'  validator={Validator.validateEmail} />
            <Input label='Пароль' type='password' name='password'  validator={Validator.validatePassword} />
            <Combobox name="typeId" label='Тип учетной записи' options={userTypeOptions} changeHandler={toggleUserType} />
            {!isAdult && <Input label='Полное имя родителя' type='text' name='parentName'  />}
            <Input onChange={PhoneChange} maxlength={13} label='Телефон' type='tel' name='phone'  validator={Validator.validatePhoneNumber} />
            <Input label='Фамилия участника' type='text' name='lastName'  maxlength={40} />
            <Input label='Имя участника' type='text' name='firstName'  maxlength={40} />
            <Input label='Отчество участника' type='text' name='patronymic'  maxlength={40} />
            <Input label='Учебное заведение' type='text' name='institution'/>
            {
                isAdult ?
                    <>
                        <Input label='Курс' type='number' name='grade' validator={Validator.validateGradeStudent} min={1} max={11} />
                        <Input label='Специальность' type='text' name='speciality' />
                    </> :
                    <Input label='Класс' type='number' name='grade' validator={Validator.validateGradeSchool} min={1} max={11} />
            }
            <Combobox name="caseId" label='Направление' options={cases} />
            <FileInput label='Согласие на обработку персональных данных' name='consent' accept={['.pdf']} />
            <FileInput label='Выполненное задание' name='solution' accept={['.pdf']} />
        </>
    )

}
export default CreateUserForm