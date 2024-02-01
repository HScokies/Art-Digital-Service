import { useEffect, useState } from "react"
import { IRole } from "src/interfaces"
import { Option } from "src/components/combobox"
import { API, Validator } from "src/services";
import { Input, Combobox } from "components/index";

const CreateStaffForm = () => {
    const [roleOptions, setRoles] = useState<Option[]>([]);

    useEffect(() => {
        const fetchRoles = async() => {
            const response = await API.getStaffRoles();
            if (response.status != 200) return;
            const data = response.data as IRole[];
            const rolesAsOptions: Option[] = []
            for (let role of data){
                rolesAsOptions.push({
                    label: role.name,
                    value: role.id
                });
            }
            setRoles(rolesAsOptions)
        }
        fetchRoles();
    }, [])

    return(
        <>
        <Input label='Адрес электронной почты' type='email' name='email'  validator={Validator.validateEmail} required />
        <Input label='Пароль' type='password' name='password'  validator={Validator.validatePassword} required />
        <Input label='Фамилия сотрудника' type='text' name='lastName'  maxlength={20} required />
        <Input label='Имя сотрудника' type='text' name='firstName'  maxlength={20} required />
        <Input label='Отчество сотрудника' type='text' name='patronymic'  maxlength={20} required />
        <Combobox name="roleId" label='Роль' options={roleOptions} />
        </>
    )
}
export default CreateStaffForm