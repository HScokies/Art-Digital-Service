import { useContext, useEffect, useState } from "react"
import { Button, Combobox, DataGridView, FileInput, Input } from "src/components"
import { Option } from "src/components/combobox"
import { formProps } from "src/components/dataGridView/Modals/createUpdateDialog"
import { param } from "src/components/dataGridView/interfaces"
import { IUserData, IUserType } from "src/interfaces"
import { API, Validator } from "src/services"

const UpdateUser = ({ id }: formProps) => {
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

    const phoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const phone = e.target.value
        let startAdding = false
        let newPhone = "+7 "

        for (let i = 0; i < phone.length; i += 1) {
            if (phone[i] == '+' && phone[i + 1] == '7' && phone[i + 2] == ' ') {
                startAdding = true;
                i += 2;
                continue;
            }

            if (!startAdding)
                continue;

            if (!isNaN(+phone[i]) && phone[i] != ' ')
                newPhone += phone[i];
        }
        e.target.value = newPhone;
    }

    const validateGrade = (e: React.KeyboardEvent<HTMLInputElement>) => {
        const numberInput = e.target as HTMLInputElement;
        if (!numberInput.value.match(/^-?\d+$/)) {
            numberInput.value = ""
        }
        else numberInput.value = (+numberInput.value).toString()
    }

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
                    {!isAdult && <Input defaultValue={userData.parentName} label='Полное имя родителя' type='text' name='parentName' required={true} />}
                    <Input defaultValue={userData.email} label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} />
                    <Input defaultValue={userData.phone} onChange={phoneChange} maxlength={13} label='Телефон' type='tel' name='phone' required={true} validator={Validator.validatePhoneNumber} />
                    <Input defaultValue={userData.lastName} label='Фамилия участника' type='text' name='lastName' required={true} maxlength={40} />
                    <Input defaultValue={userData.firstName} label='Имя участника' type='text' name='firstName' required={true} maxlength={40} />
                    <Input defaultValue={userData.patronymic} label='Отчество участника' type='text' name='patronymic' required={true} maxlength={40} />
                    <Input defaultValue={userData.city} label='Город участника' type='text' name='city' required={true} />
                    <Input defaultValue={userData.institution} label='Учебное заведение' type='text' name='institution' required={true} />
                    {
                        isAdult ?
                            <>
                                <Input defaultValue={userData.grade} onKeyUp={validateGrade} label='Курс' type='number' name='grade' required={true} validator={Validator.validateGradeStudent} min={1} max={11} />
                                <Input defaultValue={userData.speciality} label='Специальность' type='text' name='speciality' required={true} />
                            </> :
                            <Input defaultValue={userData.grade} onKeyUp={validateGrade} label='Класс' type='number' name='grade' required={true} validator={Validator.validateGradeSchool} min={1} max={11} />
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
const UpdateUser_limited = ({ id }: formProps) => {
    const [userData, setUserData] = useState<IUserData | null>(null)
    const [userStatusOptions, setUserStatusOptions] = useState<Option[]>([])
    useEffect(() => {
        const getUserData = () => {
            const data = API.getUser(id!)
            setUserData(data)
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
        getUserStatuses()
        getUserData()
    }, [id])
    return (
        <>
            {
                userData &&
                <>
                    <div className="aside-dialog-filebuttons">
                        <Button type="button" clickHandler={() => API.getFile(userData?.consentId)} isActive={true} variant='passive'>Согласие</Button>
                        <Button  type="button" clickHandler={() => API.getFile(userData?.consentId)} isActive={true} variant='passive'>Работа</Button>
                    </div>
                    <Input min={0} label='Балл' type='number' name='score' />
                    <Combobox name="statusId" defaultValue={userData.status} label='Статус' options={userStatusOptions} />
                </>
            }</>
    )
}

const CreateUser = () => {
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

    const phoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const phone = e.target.value
        let startAdding = false
        let newPhone = "+7 "

        for (let i = 0; i < phone.length; i += 1) {
            if (phone[i] == '+' && phone[i + 1] == '7' && phone[i + 2] == ' ') {
                startAdding = true;
                i += 2;
                continue;
            }

            if (!startAdding)
                continue;

            if (!isNaN(+phone[i]) && phone[i] != ' ')
                newPhone += phone[i];
        }
        e.target.value = newPhone;
    }

    const validateGrade = (e: React.KeyboardEvent<HTMLInputElement>) => {
        const numberInput = e.target as HTMLInputElement;
        if (!numberInput.value.match(/^-?\d+$/)) {
            numberInput.value = ""
        }
        else numberInput.value = (+numberInput.value).toString()
    }

    const toggleUserType = (e: React.ChangeEvent<HTMLSelectElement>) => {
        if (!userTypes) return
        const newID = +e.target.value
        const userType = userTypes.find(ut => ut.id == newID)
        if (!userType) return
        setIsAdult(userType?.isAdult)
    }

    return (

        <>
            <Input label='Адрес электронной почты' type='email' name='email' required={true} validator={Validator.validateEmail} />
            <Input label='Пароль' type='password' name='password' required={true} validator={Validator.validatePassword} />
            <Combobox name="typeId" label='Тип учетной записи' options={userTypeOptions} changeHandler={toggleUserType} />
            {!isAdult && <Input label='Полное имя родителя' type='text' name='parentName' required={true} />}
            <Input onChange={phoneChange} maxlength={13} label='Телефон' type='tel' name='phone' required={true} validator={Validator.validatePhoneNumber} />
            <Input label='Фамилия участника' type='text' name='lastName' required={true} maxlength={40} />
            <Input label='Имя участника' type='text' name='firstName' required={true} maxlength={40} />
            <Input label='Отчество участника' type='text' name='patronymic' required={true} maxlength={40} />
            <Input label='Город участника' type='text' name='city' required={true} />
            <Input label='Учебное заведение' type='text' name='institution' required={true} />
            {
                isAdult ?
                    <>
                        <Input onKeyUp={validateGrade} label='Курс' type='number' name='grade' required={true} validator={Validator.validateGradeStudent} min={1} max={11} />
                        <Input label='Специальность' type='text' name='speciality' required={true} />
                    </> :
                    <Input onKeyUp={validateGrade} label='Класс' type='number' name='grade' required={true} validator={Validator.validateGradeSchool} min={1} max={11} />
            }
            <Combobox name="caseId" label='Направление' options={cases} />
            <FileInput label='Согласие на обработку персональных данных' name='consent' accept={['.pdf']} />
            <FileInput label='Выполненное задание' name='solution' accept={['.pdf']} />
        </>
    )
}

/**
 * Если updatePermission = false
 * Пользователь может только изменять балл и скачивать файл
 */
interface props {
    createPermission: boolean,
    updatePermission: boolean,
    deletePermission: boolean
}

const Users = ({ createPermission, updatePermission, deletePermission }: props) => {
    const [userTypeFilter, setUserTypeFilter] = useState<param[]>([])
    const [casesFilter, setCasesFilter] = useState<param[]>([])
    useEffect(() => {
        const createUserTypesFilter = () => {
            const data = API.getUserTypes()
            let filter: param[] = []
            for (let userType of data) {
                filter.push({
                    name: "excludeUserType",
                    title: userType.name,
                    value: userType.id
                })
            }
            setUserTypeFilter(filter)
        }
        const createCasesFilter = () => {
            const data = API.getCases()
            let filter = []
            for (let _case of data) {
                filter.push({
                    name: "excludeCase",
                    title: _case.name,
                    value: _case.id
                })
            }
            setCasesFilter(filter)
        }
        createUserTypesFilter()
        createCasesFilter()
    }, [])
    return (
        <DataGridView
            searchLabel="Участник"
            dataSource={API.getUsers}
            exportProvider={API.exportUsers}
            deleteProvider={deletePermission ? API.deleteUsers : undefined}
            createProvider={createPermission ? API.createUser : undefined}
            createForm={createPermission ? CreateUser : undefined}
            updateProvider={API.updateUser}
            updateForm={updatePermission ? UpdateUser : UpdateUser_limited}
            rowsPerPageOptions={new Set([5, 10, 25, 50, 75])}
            columns={[
                {
                    id: "users",
                    title: "Участник",
                },
                {
                    id: "userType",
                    title: "Тип",
                    filters: userTypeFilter
                },
                {
                    id: "cases",
                    title: "Направление",
                    filters: casesFilter
                },
                {
                    id: "score",
                    title: "Балл",
                    filters: [
                        {
                            title: 'Есть значение',
                            name: 'hasScore',
                            value: false,
                        },
                        {
                            title: 'Нет значения',
                            name: 'noScore',
                            value: false,
                        }
                    ]
                }
            ]}
        />
    )
}


export default Users