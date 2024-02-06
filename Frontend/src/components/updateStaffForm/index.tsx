import { useEffect, useState } from "react";
import { IRole, IStaffData, IUpdateForm } from "src/interfaces";
import { Input, Combobox } from "components/index";
import { Option } from "src/components/combobox"
import { API, Validator } from "src/services";

const UpdateStaffForm = ({ formId, entityId }: IUpdateForm) => {
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

            const staff = await API.getStaffById(entityId);
            setStaff(staff);

        }
        fetch();
    }, [])

    const handleSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        await API.updateStaff(entityId, new FormData(e.target as HTMLFormElement))
    }
    
    return (
        !staff? <></> :
        <form id={formId} onSubmit={(e) => handleSubmit(e)}>
            <Input label='Адрес электронной почты' type='email' name='email' validator={Validator.validateEmail} defaultValue={staff?.email} required />
            <Input label='Фамилия сотрудника' type='text' name='lastName' maxlength={20} defaultValue={staff?.lastName} required />
            <Input label='Имя сотрудника' type='text' name='firstName' maxlength={20} defaultValue={staff?.firstName} required />
            <Input label='Отчество сотрудника' type='text' name='patronymic' maxlength={20} defaultValue={staff?.patronymic} required />
            <Combobox name="roleId" label='Роль' options={roleOptions} defaultValue={staff?.roleId} />
        </form>
    );
}

export default UpdateStaffForm;