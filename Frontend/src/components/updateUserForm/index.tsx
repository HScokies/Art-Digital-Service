import { useEffect, useState } from "react"
import { formProps } from "../dataGridView/Modals/createUpdateDialog"
import { Option } from "src/components/combobox"
import { ICity, IUserData, IUserType } from "src/interfaces"
import { API, PhoneChange } from "src/services"
import { Combobox, Input, FileInput } from ".."

const UpdateUserForm = ({ id }: formProps) => {
    const [isAdult, setIsAdult] = useState(true)
    const [userData, setUserData] = useState<IUserData | null>(null)
    const [cases, setCases] = useState<Option[]>([])
    const [userTypes, setUserTypes] = useState<IUserType[]>()
    const [userTypeOptions, setUserTypeOptions] = useState<Option[]>([])
    const [userStatusOptions, setUserStatusOptions] = useState<Option[]>([])
    const [cities, setCities] = useState<ICity[]>()
    useEffect(() => {
        const fetch = async() => {
            const cases = (await API.getCases()).data
            let casesAsOptions: Option[] = []
            for (let _case of cases) {
                casesAsOptions.push({
                    value: _case.id,
                    label: _case.name
                })
            }
            setCases(casesAsOptions)

            const statuses = await API.getUserStatuses()
            let userStatusOptions: Option[] = []
            for (let userStatus of statuses) {
                userStatusOptions.push({
                    value: userStatus.id,
                    label: userStatus.name
                })
            }
            setUserStatusOptions(userStatusOptions)

            const types = (await API.getParticipantTypes()).data
            setUserTypes(types)
            let userTypeOptions: Option[] = []
            for (let userType of types) {
                userTypeOptions.push({
                    value: userType.id,
                    label: userType.name
                })
            }
            setUserTypeOptions(userTypeOptions)

            const userData = await API.getUser(id!)
            if (!userData) return;
            setUserData(userData)

            const userType = (types as IUserType[])?.find(t => t.id == userData.typeId);
            if (userType)
                setIsAdult(userType.isAdult)
        }
        if (id == -1) {
            setUserData(null)
            return
        }
        fetch();
    }, [id])

    useEffect(() => {
        const fetch = async() => {
            const response = await API.getCities()
            if (response.status != 200) return;
            const data = response.data;
            setCities(data)
        }
        fetch()
    },[])


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
                    <Combobox name="userTypeId" defaultValue={userData.typeId} label='Тип учетной записи' options={userTypeOptions} changeHandler={toggleUserType} />
                    {!isAdult && <Input defaultValue={userData.parentName} required label='Полное имя родителя' type='text' name='parentName'  />}
                    <Input defaultValue={userData.email} required label='Адрес электронной почты' type='email' name='email' />
                    <Input defaultValue={userData.phone} required onChange={PhoneChange} maxlength={13} label='Телефон' type='tel' name='phone'/>
                    <Input defaultValue={userData.lastName} required label='Фамилия участника' type='text' name='lastName'  maxlength={40} />
                    <Input defaultValue={userData.firstName} required label='Имя участника' type='text' name='firstName'  maxlength={40} />
                    <Input defaultValue={userData.patronymic} required label='Отчество участника' type='text' name='patronymic'  maxlength={40} />
                    <Input defaultValue={userData.city} required datalist="Cities" label='Город участника' type='text' name='city'/>
                    <datalist id="Cities">
                        {
                            cities?.map((e) => 
                            <option value={e.name} key={e.id}></option>
                            )
                        }
                    </datalist>
                    <Input defaultValue={userData.institution} required label='Учебное заведение' type='text' name='institution'  />
                    {
                        isAdult ?
                            <>
                                <Input defaultValue={userData.grade} required label='Курс' type='number' name='grade'  min={1} max={11} />
                                <Input defaultValue={userData?.speciality} required label='Специальность' type='text' name='speciality'  />
                            </> :
                            <Input defaultValue={userData.grade} required label='Класс' type='number' name='grade' min={1} max={11} />
                    }
                    <Combobox name="caseId" defaultValue={userData.caseId} label='Направление' options={cases} />
                    <FileInput initialFileName={userData?.consentFilename} downloadLink={`${API.URL}files/user-uploaded/${userData?.consentFilename}?displayedName="Согласие_${userData.lastName}"`} label='Согласие на обработку персональных данных' name='consent' accept={['.pdf']} />
                    <FileInput initialFileName={userData?.solutionFilename} downloadLink={`${API.URL}files/user-uploaded/${userData?.solutionFilename}?displayedName="Решение_${userData.lastName}"`} label='Выполненное задание' name='solution' accept={['.pdf']} />
                    <Input defaultValue={userData?.rating} min={0} label='Балл' type='number' name='rating' />
                    <Combobox name="status" defaultValue={userData.status} label='Статус' options={userStatusOptions} />
                </>
            }
        </>
    )
}
export default UpdateUserForm