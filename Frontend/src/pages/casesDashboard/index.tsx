import { useState, useRef, useEffect } from "react"
import { Button } from "src/components"
import { ICase } from "src/interfaces"
import { API } from "src/services"
import Icons from "images/icons.svg"
import { NavLink, useNavigate } from "react-router-dom"

const CasesDashboardPage = () => {
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

    const handleDelete = async() => {
        if (id){
            await API.dropCase(id)
            setTrigger(!trigger);
        }
        dialog.current?.close();
    }

    return (
        <div className="cases">
            <div className="controls">
                <input className="searchbar" type="text" placeholder="Направление" onChange={(e) => setSearch(e.target.value)} />
                <Button clickHandler={() => navigate('/dashboard/cases/add')} isActive={true}>Добавить</Button>
            </div>
            <div className="cases-wrapper">
                {
                    cases.map((c) => (
                        <div key={c.id} className="cases-wrapper_element">
                            {c.name}
                            <div className="icon-wrapper">
                                <NavLink to={`/dashboard/cases/${c.id}`}>
                                    <svg>
                                        <use xlinkHref={Icons + '#expand'} />
                                    </svg>
                                </NavLink>
                                <svg id="drop" onClick={() => OpenDialog(c.id)}>
                                    <use xlinkHref={Icons + "#trash"} />
                                </svg>
                            </div>
                        </div>
                    ))
                }
            </div>

            <dialog ref={dialog} id='delete-row-dialog'>
                <div className="title">
                    Подтвердите удаление направления
                </div>
                <div className='descr'>
                    <p>Вы уверены, что хотите удалить выбранное направление?</p>
                    <p>Это действие не может быть отменено.</p>
                </div>
                <div className='button-container'>
                    <Button variant='passive' clickHandler={() => dialog.current?.close()} isActive={true}>Отмена</Button>
                    <Button clickHandler={() => handleDelete()} variant='danger' isActive={true}>Удалить</Button>
                </div>
            </dialog>
        </div>
    )
}
export default CasesDashboardPage