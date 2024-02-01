import { useEffect, useState } from "react";
import { formProps } from "../dataGridView/Modals/createUpdateDialog"
import { IRole, IStaffData } from "src/interfaces";
import { Input, Combobox } from "components/index";
import { Option } from "src/components/combobox"
import { API, Validator } from "src/services";

const UpdateStaffForm = ({ id }: formProps) => {
    const [staff, setStaff] = useState<IStaffData | null>(null)
    const [roleOptions, setRoles] = useState<Option[]>([]);
    useEffect(() => {
        const fetch = async () => {
            const response = await API.getStaffRoles();
            if (response.status != 200) return;
            const data = response.data as IRole[];
            const rolesAsOptions: Option[] = []
            for (let role of data) {
                rolesAsOptions.push({
                    label: role.name,
                    value: role.id
                });
            }
            setRoles(rolesAsOptions)

            const staff = await API.getStaffById(id!);
            setStaff(staff);

        }
        if (id == -1) {
            setStaff(null)
            return
        }
        fetch();
    }, [id])

    return (
        <>
            <Input label='Адрес электронной почты' type='email' name='email' validator={Validator.validateEmail} defaultValue={staff?.email} required />
            <Input label='Фамилия сотрудника' type='text' name='lastName' maxlength={20} defaultValue={staff?.lastName} required />
            <Input label='Имя сотрудника' type='text' name='firstName' maxlength={20} defaultValue={staff?.firstName} required />
            <Input label='Отчество сотрудника' type='text' name='patronymic' maxlength={20} defaultValue={staff?.patronymic} required />
            <Combobox name="roleId" label='Роль' options={roleOptions} defaultValue={staff?.roleId} />
        </>
    );
}

export default UpdateStaffForm;