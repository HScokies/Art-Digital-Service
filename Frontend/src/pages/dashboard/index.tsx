import { useEffect, useState } from 'react'
import './style.scss'
import { AsideMenu } from 'src/components'
import { menuElement } from 'src/components/asideMenu'
import { API } from 'src/services'
import { Outlet } from 'react-router-dom'
import { UsePermissions } from 'src/hooks/usePermissions'


const DashboardPage = () => {
    const [navElements, setNavElements] = useState<menuElement[]>([])
    const { setPermissions, permissions } = UsePermissions()

    useEffect(() => {
        const fetch = async () => {
            const response = await API.getPremissions()
            if (response.status != 200) return;
            const premissionsData = response.data;
            setPermissions(premissionsData);
        }
        fetch()
    }, [])

    useEffect(() => {
        let elements = []
        if (permissions?.readUsers) {
            elements.push(
                {
                    iconId: '#users',
                    title: 'Участники',
                    to: 'dashboard/users'
                }
            )
        }
        if (permissions?.readCases) {
            elements.push({
                iconId: '#book',
                title: 'Направления',
                to: 'dashboard/cases'
            })
        }
        if (permissions?.readStaff) {
            elements.push(
                {
                    iconId: '#shield',
                    title: 'Сотрудники',
                    to: 'dashboard/staff'
                })
        }
        if (permissions?.utilsAccess){
            elements.push(
                {
                    iconId: '#other',
                    title: 'Прочее',
                    to: 'dashboard/utils'
                }
            )
        }
        setNavElements(elements)
    }, [permissions])

    return (
            <div className='dashboardpage'>
                <AsideMenu
                    items={navElements}
                />
                <section className='dashboardpage-content'>
                    <Outlet />
                </section>
            </div>
    )
}

export default DashboardPage