import { DataGridView } from "src/components"
import { formProps } from "src/components/dataGridView/Modals/createUpdateDialog"
import { API } from "src/services"

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

const StaffDashboardPage = () => {
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
export default StaffDashboardPage