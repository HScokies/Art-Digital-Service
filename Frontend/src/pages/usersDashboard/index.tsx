import { useEffect, useState } from "react"
import { CreateUserForm, DataGridView, RateUserForm, UpdateUserForm } from "src/components"
import { param } from "src/components/dataGridView/interfaces"
import { API } from "src/services"



const UsersDashboardPage = () => {
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


    return(
        <DataGridView
        searchLabel="Участник"
        dataSource={API.getUsers}
        exportProvider={API.exportUsers}
        deleteProvider={API.deleteUsers}
        createProvider={API.createUser}
        createForm={CreateUserForm}
        updateProvider={API.updateUser}
        updateForm={RateUserForm}
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
export default UsersDashboardPage