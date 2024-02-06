import { API } from "src/services";
import { FileInput, Input } from "..";
import { useEffect, useState } from "react";
import { ICertificateConfig } from "src/interfaces";


const CertificateForm = () => {
    const onSubmit = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        await API.appendCertificateSettings(new FormData(e.target as HTMLFormElement))
    }

    const [certificateConfig, setConfig] = useState<ICertificateConfig>()
    useEffect(() => {
        const fetch = async() => {
            const response = await API.getCertificateConfig()
            if (response.status != 200) return;
            setConfig(response.data)
        }
        fetch()
    }, [])

    return(
        <form id="update-certificate" onSubmit={(e) => onSubmit(e)}>
            <FileInput name="blank" label="Бланк" accept={[".png", ".jpg"]} downloadLink={API.URL+"files/certificate"} />
            <Input name="paddingLeft" label="Отступ слева" type="number" defaultValue={certificateConfig?.paddingLeft}/>  
            <Input name="paddingTop" label="Отступ сверху" type="number" defaultValue={certificateConfig?.paddingTop}/>  
            <Input name="paddingRight" label="Отступ справа" type="number" defaultValue={certificateConfig?.paddingRight}/>            
            <Input name="paddingBottom" label="Отступ снизу" type="number" defaultValue={certificateConfig?.paddingBottom}/>                
        </form>
    );
}
export default CertificateForm