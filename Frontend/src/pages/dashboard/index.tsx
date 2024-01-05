import { useEffect, useState } from 'react'
import './style.scss'
import { AsideMenu } from 'src/components'
import { menuElement } from 'src/components/asideMenu'
import { API } from 'src/services'
import { IPermissions } from 'src/interfaces'
import { Outlet } from 'react-router-dom'


const DashboardPage = () => {
    const [navElements, setNavElements] = useState<menuElement[]>([])
    const [permissions, setPermissions] = useState<IPermissions>()

    useEffect(() => {
        setPermissions(API.getPremissions())
    }, [])

    useEffect(() => {
        let elements = []
        if (permissions?.users.read){
            elements.push(
                {
                    iconId: '#users',
                    title: 'Участники',
                    to: 'dashboard/users'
                }
            )
        }
        if (permissions?.cases.read){
            elements.push({
                iconId: '#book',
                title: 'Направления',
                to: 'dashboard/cases'
            })
        }
        if (permissions?.staff.read){
            elements.push(
                {
                    iconId: '#shield',
                    title: 'Сотрудники',
                    to: 'dashboard/staff'
                })
        }
        setNavElements(elements)
    }, [permissions])

    return (
        <div className='dashboardpage'>
            <AsideMenu
                items={navElements}
            />
            <section className='dashboardpage-content'>
                <Outlet/>
            </section>
        </div>
    )
}

export default DashboardPage