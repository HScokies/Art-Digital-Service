import { useEffect, useState } from "react"
import { CreateUserForm, DataGridView, RateUserForm, UpdateUserForm } from "src/components"
import { param } from "src/components/dataGridView/interfaces"
import { API } from "src/services"



const UsersDashboardPage = () => {
    const [userTypeFilter, setUserTypeFilter] = useState<param[]>([])
    const [casesFilter, setCasesFilter] = useState<param[]>([])

    useEffect(() => {                
        const createUserTypesFilter = async() => {
            const data = (await API.getParticipantTypes()).data
            let filter: param[] = []
            for (let userType of data) {
                filter.push({
                    name: "excludeType",
                    title: userType.name,
                    value: userType.id
                })
            }
            await setUserTypeFilter(filter)
        }
        const createCasesFilter = async() => {
            const response = await API.getCases();
            if (response.status != 200) return;
            const data = response.data;

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


    return(
        <DataGridView
        searchLabel="Участник"
        dataSource={API.getUsers}
        exportProvider={API.exportParticipants}
        deleteProvider={API.deleteUsers}
        createProvider={API.createUser}
        createForm={CreateUserForm}
        updateProvider={API.updateUser}
        updateForm={UpdateUserForm}
        rowsPerPageOptions={new Set([5, 10, 15, 20, 25])}
        columns={[
            {
                id: "name",
                title: "Участник",
                sizePx: 882
            },
            {
                id: "type",
                title: "Тип",
                filters: userTypeFilter,
                sizePx: 455
            },
            {
                id: "case",
                title: "Направление",
                filters: casesFilter,
                sizePx: 751
            },
            {
                id: "rating",
                title: "Балл",
                sizePx: 50,
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
export default UsersDashboardPage