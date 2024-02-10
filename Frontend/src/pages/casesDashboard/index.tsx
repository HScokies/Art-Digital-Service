import './style.scss'
import { useState, useRef, useEffect } from "react"
import { Button, DialogConfirm, MenuCard } from "src/components"
import { ICase } from "src/interfaces"
import { API } from "src/services"
import { useNavigate } from "react-router-dom"
import { UsePermissions } from "src/hooks/usePermissions"

const CasesDashboardPage = () => {
    const { permissions } = UsePermissions()

    const navigate = useNavigate()
    const [cases, setCases] = useState<ICase[]>([])
    const dialog = useRef<HTMLDialogElement>(null);
    const [id, setId] = useState<number>()
    const [search, setSearch] = useState<string>()
    const [trigger, setTrigger] = useState(false)

    useEffect(() => {
        const fetch = async () => {
            const response = await API.getCases(search);
            if (response.status != 200) return;
            setCases(response.data)
        }
        fetch()
    }, [search, trigger])

    const OpenDialog = (id: number) => {
        setId(id);
        dialog.current?.showModal();
    }

    const handleDelete = async () => {
        if (id) {
            await API.dropCase(id)
            setTrigger(!trigger);
        }
        dialog.current?.close();
    }

    return (
        <div className="cases">
            <div className="controls">
                <input className="searchbar" type="text" placeholder="Направление" onChange={(e) => setSearch(e.target.value)} />
                {
                    permissions?.createCases && <Button clickHandler={() => navigate('/dashboard/cases/add')} isActive={true}>Добавить</Button>
                }
            </div>
            <div className="cases-wrapper">
                {
                    cases.map((c) => (
                        <MenuCard key={c.id} title={c.name} onExpand={() => navigate(permissions?.updateCases ? `/dashboard/cases/${c.id}` : '/dashboard/cases')} onDelete={() => permissions?.deleteCases && OpenDialog(c.id)} />
                    ))
                }
            </div>

            <dialog ref={dialog} className='confirm-dialog'>
                <DialogConfirm title='Подтвердите удаление направления' acceptText='Удалить' acceptStyle='danger' dialog={dialog.current} onAccept={() => handleDelete()}>
                    <p>Вы уверены, что хотите удалить выбранное направление?</p>
                    <p>Это действие не может быть отменено.</p>
                </DialogConfirm>
            </dialog>
        </div>
    )
}
export default CasesDashboardPage