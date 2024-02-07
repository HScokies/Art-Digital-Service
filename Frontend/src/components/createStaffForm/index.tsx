import { useEffect, useState } from "react"
import { ICreateForm, IRole } from "src/interfaces"
import { Option } from "src/components/combobox"
import { API, Validator } from "src/services";
import { Input, Combobox } from "components/index";

const CreateStaffForm = ({formId}: ICreateForm) => {
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

    const handleSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        await API.createStaff(new FormData(e.target as HTMLFormElement))
    }

    return(
        <form id={formId} onSubmit={(e) => handleSubmit(e)}>
        <Input label='Адрес электронной почты' type='email' name='email'  validator={Validator.validateEmail} required />
        <Input label='Пароль' type='password' name='password'  validator={Validator.validatePassword} required />
        <Input label='Фамилия сотрудника' type='text' name='lastName'  maxlength={17} required />
        <Input label='Имя сотрудника' type='text' name='firstName'  maxlength={17} required />
        <Input label='Отчество сотрудника' type='text' name='patronymic'  maxlength={20} required />
        <Combobox name="roleId" label='Роль' options={roleOptions} />
        </form>
    )
}
export default CreateStaffForm