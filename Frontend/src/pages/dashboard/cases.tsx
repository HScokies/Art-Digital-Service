import { useEffect, useState } from "react"
import { NavLink } from "react-router-dom"
import { Button } from "src/components"
import { API } from "src/services"
import Icons from 'images/icons.svg'
import { Pages } from "src/enums"

interface ICase {
    id: number,
    name: string
}

const CaseElement = ({id, name}: ICase) => (
    <NavLink to={Pages.cases + id} className="cases-wrapper_element">
        {name}
        <svg>
            <use xlinkHref={Icons + '#expand'}/>
        </svg>
    </NavLink>
)

const Cases = () => {
    const [cases, setCases] = useState<ICase[]>([])
    useEffect(() => {
        setCases(API.getCases())
    }, [])
    return (
        <div className="cases">
            <div className="controls">
                <input className="searchbar" type="text" placeholder="Направление" />
                <Button isActive={true}>Добавить</Button>
            </div>
            <div className="cases-wrapper">
                {
                    cases.map((c) => (
                        <CaseElement key={c.id} id={c.id} name={c.name}/>
                    ))
                }
            </div>
        </div>
    )
}
export default Cases