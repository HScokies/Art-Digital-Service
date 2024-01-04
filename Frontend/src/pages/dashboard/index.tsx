import { useEffect, useState } from 'react'
import './style.scss'
import { AsideMenu } from 'src/components'
import Users from './users'
import Staff from './staff'
import { menuElement } from 'src/components/asideMenu'
import { API } from 'src/services'
import { IPermissions } from 'src/interfaces'
import Cases from './cases'


const DashboardPage = () => {
    const [content, setContent] = useState<JSX.Element>()
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
                    onClick: () => setContent(<Users 
                        createPermission={permissions.users.create} 
                        updatePermission={permissions.users.update}
                        deletePermission={permissions.users.delete}
                        />)
                }
            )
        }
        if (permissions?.cases.read){
            elements.push({
                iconId: '#book',
                title: 'Направления',
                onClick: () => setContent(<Cases/>)
            })
        }
        if (permissions?.staff.read){
            elements.push(
                {
                    iconId: '#shield',
                    title: 'Сотрудники',
                    onClick: () => setContent(<Staff />)
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
                {content}
            </section>
        </div>
    )
}

export default DashboardPage