import { useEffect, useState } from "react"
import { formProps } from "../dataGridView/Modals/createUpdateDialog"
import { Option } from "src/components/combobox"
import { IUserData, IUserType } from "src/interfaces"
import { API, PhoneChange } from "src/services"
import { Combobox, Input, FileInput } from ".."

const UpdateUserForm = ({ id }: formProps) => {
    const [isAdult, setIsAdult] = useState(true)
    const [userData, setUserData] = useState<IUserData | null>(null)
    const [cases, setCases] = useState<Option[]>([])
    const [userTypes, setUserTypes] = useState<IUserType[]>()
    const [userTypeOptions, setUserTypeOptions] = useState<Option[]>([])
    const [userStatusOptions, setUserStatusOptions] = useState<Option[]>([])
    useEffect(() => {
        const getUserData = () => {
            const data = API.getUser(id!)
            setUserData(data)
            setIsAdult(data.userType.isAdult)
        }
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
        const getUserStatuses = () => {
            const data = API.getUserStatuses()
            let userStatusOptions: Option[] = []
            for (let userStatus of data) {
                userStatusOptions.push({
                    value: userStatus.id,
                    label: userStatus.name
                })
            }
            setUserStatusOptions(userStatusOptions)
        }

        if (id == -1) {
            setUserData(null)
            return
        }
        getCases()
        getUserTypes()
        getUserStatuses()
        getUserData()
    }, [id])


    const toggleUserType = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (!userTypes) return
        const newID = +e.target.value
        const userType = userTypes.find(ut => ut.id == newID)
        if (!userType) return
        setIsAdult(userType?.isAdult)
    }

    return (

        <>
            {
                userData &&
                <>
                    <Combobox name="typeId" defaultValue={userData.userType.id} label='Тип учетной записи' options={userTypeOptions} changeHandler={toggleUserType} />
                    {!isAdult && <Input defaultValue={userData.parentName} label='Полное имя родителя' type='text' name='parentName'  />}
                    <Input defaultValue={userData.email} label='Адрес электронной почты' type='email' name='email' />
                    <Input defaultValue={userData.phone} onChange={PhoneChange} maxlength={13} label='Телефон' type='tel' name='phone'/>
                    <Input defaultValue={userData.lastName} label='Фамилия участника' type='text' name='lastName'  maxlength={40} />
                    <Input defaultValue={userData.firstName} label='Имя участника' type='text' name='firstName'  maxlength={40} />
                    <Input defaultValue={userData.patronymic} label='Отчество участника' type='text' name='patronymic'  maxlength={40} />
                    <Input defaultValue={userData.city} label='Город участника' type='text' name='city'  />
                    <Input defaultValue={userData.institution} label='Учебное заведение' type='text' name='institution'  />
                    {
                        isAdult ?
                            <>
                                <Input defaultValue={userData.grade} label='Курс' type='number' name='grade'  min={1} max={11} />
                                <Input defaultValue={userData.speciality} label='Специальность' type='text' name='speciality'  />
                            </> :
                            <Input defaultValue={userData.grade} label='Класс' type='number' name='grade' min={1} max={11} />
                    }
                    <Combobox name="caseId" defaultValue={userData.case.id} label='Направление' options={cases} />
                    <FileInput initialFileName={userData?.consentId} label='Согласие на обработку персональных данных' name='consent' accept={['.pdf']} />
                    <FileInput initialFileName={userData?.solutionId} label='Выполненное задание' name='solution' accept={['.pdf']} />
                    <Input min={0} label='Балл' type='number' name='score' />
                    <Combobox name="statusId" defaultValue={userData.status} label='Статус' options={userStatusOptions} />
                </>
            }
        </>
    )
}
export default UpdateUserForm