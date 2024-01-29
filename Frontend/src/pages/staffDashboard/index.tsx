import { useEffect, useState } from "react"
import { CreateStaffForm, DataGridView, UpdateStaffForm } from "src/components"
import { param } from "src/components/dataGridView/interfaces"
import { UsePermissions } from "src/hooks/usePermissions"
import { IRole } from "src/interfaces"
import { API } from "src/services"



const StaffDashboardPage = () => {
    const [rolesFilter, setRoles] = useState<param[]>([])
    const { permissions } = UsePermissions()

    useEffect(() => {        
        const fetchRoles = async() => {
             const response = await API.getStaffRoles();
             if (response.status != 200) return;
             const data = response.data as IRole[];
             const filter:param[] = []
             for (let role of data){
                filter.push({
                    name: "excludeRole",
                    title: role.name,
                    value: role.id
                })
             }
             setRoles(filter);
        }
        fetchRoles()
    },[])


    return(
        <DataGridView
            searchLabel="Сотрудник"
            dataSource={API.getStaff}
            exportProvider={API.exportStaff}
            deleteProvider={permissions?.deleteStaff ? API.deleteStaff : undefined}
            updateProvider={API.updateStaff}
            createProvider={API.createStaff}
            createForm={permissions?.createStaff? CreateStaffForm : undefined}
            updateForm={permissions?.updateStaff? UpdateStaffForm : undefined}
            rowsPerPageOptions={new Set([5, 10, 15, 20, 25])}
            columns={[
                {
                    id: "name",
                    title: "Сотрудник",
                },
                {
                    id: "role",
                    title: "Роль",
                    filters: rolesFilter
                }
            ]}
        />
    )
}
export default StaffDashboardPage