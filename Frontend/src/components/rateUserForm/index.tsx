import { useState, useEffect } from "react"
import { IUserData } from "src/interfaces"
import { API } from "src/services"
import { Button, Input, Combobox } from "components/index"
import { formProps } from "../dataGridView/Modals/createUpdateDialog"
import { Option } from "src/components/combobox"

const RateUserForm = ({ id }: formProps) => {
    const [userData, setUserData] = useState<IUserData | undefined>()
    const [userStatusOptions, setUserStatusOptions] = useState<Option[]>([])
    useEffect(() => {
        const getUserData = async() => {
            const data = await API.getUser(id!)
            setUserData(data)
        }
        const getUserStatuses = async() => {
            const data = await API.getUserStatuses()
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
            setUserData(undefined)
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
                        <Button type="button" clickHandler={() => location.href = `${API.URL}files/user-uploaded/${userData?.consentFilename}?displayedName="Согласие_${userData.lastName}"`} isActive={true} variant='passive'>Согласие</Button>
                        <Button  type="button" clickHandler={() => location.href = `${API.URL}files/user-uploaded/${userData?.solutionFilename}?displayedName="Решение_${userData.lastName}"`} isActive={true} variant='passive'>Работа</Button>
                    </div>
                    <Input min={0} defaultValue={userData.rating} label='Балл' type='number' name='rating' />
                    <Combobox name="status" defaultValue={userData.status} label='Статус' options={userStatusOptions} />
                </>
            }</>
    )
}
export default RateUserForm