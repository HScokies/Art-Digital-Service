import { useState, useEffect } from "react"
import { IUserData } from "src/interfaces"
import { API } from "src/services"
import { Button, Input, Combobox } from "components/index"
import { formProps } from "../dataGridView/Modals/createUpdateDialog"
import { Option } from "src/components/combobox"

const RateUserForm = ({ id }: formProps) => {
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
                        <Button type="button" clickHandler={() => API.getFile(userData?.solutionFilename)} isActive={true} variant='passive'>Согласие</Button>
                        <Button  type="button" clickHandler={() => API.getFile(userData?.consentFilename)} isActive={true} variant='passive'>Работа</Button>
                    </div>
                    <Input min={0} label='Балл' type='number' name='rating' />
                    <Combobox name="statusId" defaultValue={userData.status} label='Статус' options={userStatusOptions} />
                </>
            }</>
    )
}
export default RateUserForm