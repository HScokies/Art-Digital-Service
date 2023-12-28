import { DataGridView } from "src/components"
import { API } from "src/services"

const Users = () => (
    <DataGridView
        searchLabel="Участник"
        dataSource={API.getUsers}
        exportProvider={API.exportUsers}
        deleteProvider={API.deleteUsers}
        rowsPerPageOptions={new Set([5,10,25,50,75])}
        columns={[
            {
                id: "users",
                title: "Участник",
            },
            {
                id: "userType",
                title: "Тип",
            },
            {
                id: "cases",
                title: "Направление",
                filters: [
                    {
                        title: '3D моделирование для компьютерных игр',
                        name: 'excludeCase',
                        value: 1,
                    },
                    {
                        title: 'Разработка компьютерных игр',
                        name: 'excludeCase',
                        value: 2,
                    },
                ]
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

export default Users