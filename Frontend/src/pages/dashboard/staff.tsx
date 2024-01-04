import { API } from 'src/services'
import './style.scss'
import { DataGridView } from 'src/components'
import { formProps } from 'src/components/dataGridView/Modals/createUpdateDialog'

const CreateEmployee = () => {
    return(
        <></>
    )
}
const UpdateEmployee = ({ id }: formProps) => {
    return(
        <></>
    )
}
/*
    Администратор - все
    Модератор {
        Полное взаимодествие с участниками
    },
    Преподаватель {
        Изменение оценки,
        Изменение статуса
    }
*/

const Staff = () => {
    return(
        <DataGridView
            searchLabel="Сотрудник"
            dataSource={API.getUsers}
            exportProvider={API.exportUsers}
            deleteProvider={API.deleteUsers}
            updateProvider={API.updateUser}
            createProvider={API.createUser}
            createForm={CreateEmployee}
            updateForm={UpdateEmployee}
            rowsPerPageOptions={new Set([5, 10, 25, 50, 75])}
            columns={[
                {
                    id: "employees",
                    title: "Сотрудник",
                },
                {
                    id: "role",
                    title: "Роль"
                }
            ]}
        />
    )
}
export default Staff