import { useState, useEffect } from "react"
import { IUpdateForm, IUserData } from "src/interfaces"
import { API } from "src/services"
import { Button, Input, Combobox } from "components/index"
import { Option } from "src/components/combobox"


const RateUserForm = ({ formId, entityId }: IUpdateForm): React.JSX.Element=> {
    const [userData, setUserData] = useState<IUserData | undefined>()
    const [userStatusOptions, setUserStatusOptions] = useState<Option[]>([])
    useEffect(() => {
        const getUserData = async () => {
            const data = await API.getUser(entityId)
            setUserData(data)
        }
        const getUserStatuses = async () => {
            const data = await API.getParticipantStatuses()
            let userStatusOptions: Option[] = []
            for (let userStatus of data) {
                userStatusOptions.push({
                    value: userStatus.id,
                    label: userStatus.name
                })
            }
            setUserStatusOptions(userStatusOptions)
        }
        getUserStatuses()
        getUserData()
    }, [])

    const onSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        await API.rateUser(entityId, new FormData(e.target as HTMLFormElement))
    }

    return (
        !userData? <></>:
            <form id={formId} onSubmit={(e) => onSubmit(e)}>
                <div className="menu_dialog-filebuttons">
                    <Button type="button" clickHandler={() => location.href = `${API.URL}files/user-uploaded/${userData?.consentFilename}?displayedName="Согласие_${userData.lastName}"`} isActive={true} variant='passive'>Согласие</Button>
                    <Button type="button" clickHandler={() => location.href = `${API.URL}files/user-uploaded/${userData?.solutionFilename}?displayedName="Решение_${userData.lastName}"`} isActive={true} variant='passive'>Работа</Button>
                </div>
                <Input min={0} defaultValue={userData.rating} label='Балл' type='number' name='rating' />
                <Combobox name="status" defaultValue={userData.status} label='Статус' options={userStatusOptions} />
            </form>
    )
}
export default RateUserForm