import './style.scss'
import { useEffect, useRef, useState } from "react"
import { CertificateForm, DialogMenu, LegalDocumentsForm, MenuCard } from "src/components"


const UtilsDashboardPage = () => {
    const [menu, setMenu] = useState<React.JSX.Element>()
    const [title, setTitle] = useState<string>('')
    const [form, setForm] = useState<string>('')
    const dialog = useRef<HTMLDialogElement>(null)

    const openMenu = (title: string, form: string, menu: React.JSX.Element) => {
        setTitle(title)
        setMenu(menu)
        setForm(form)
        dialog.current?.showModal()
    }

    useEffect(() => {
        const closeHandler = () => {
            setMenu(<></>);
        }
        dialog.current?.addEventListener('close', closeHandler)
    }, [])
    
    return(
        <div className="utils">
            <div className="utils_wrapper">
                <MenuCard title="Юридические документы" onExpand={() => openMenu("Юридические документы","update-legal",<LegalDocumentsForm/>)}/>
                <MenuCard title="Настройки сертификата" onExpand={() => openMenu("Настройки сертификата", "update-certificate", <CertificateForm/>)}/>
            </div>
            <dialog ref={dialog} className="utils-menu">
                <DialogMenu title={title} dialog={dialog.current} form={form}>
                    {menu}
                </DialogMenu>                
            </dialog>
        </div>
    )
}
export default UtilsDashboardPage