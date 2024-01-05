import { useState, useRef, useEffect } from "react"
import { Button } from "src/components"
import { ICase } from "src/interfaces"
import { API } from "src/services"
import Icons from "images/icons.svg"
import { NavLink, useNavigate } from "react-router-dom"

const CasesDashboardPage = () => {
    const navigate = useNavigate()
    const [cases, setCases] = useState<ICase[]>([])

    useEffect(() => {
        setCases(API.getCases())
    }, [])

    return (
        <div className="cases">
            <div className="controls">
                <input className="searchbar" type="text" placeholder="Направление" />
                <Button clickHandler={() => navigate('/dashboard/cases/add')} isActive={true}>Добавить</Button>
            </div>
            <div className="cases-wrapper">
                {
                    cases.map((c) => (
                        <NavLink to={`/dashboard/cases/${c.id}`} key={c.id} className="cases-wrapper_element">
                            {c.name}
                            <svg>
                                <use xlinkHref={Icons + '#expand'} />
                            </svg>
                        </NavLink>
                    ))
                }
            </div>
        </div>
    )
}
export default CasesDashboardPage