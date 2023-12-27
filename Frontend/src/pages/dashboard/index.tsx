import { useEffect, useState } from 'react'
import './style.scss'
import { AsideMenu, DataGridView } from 'src/components'
import { API } from 'src/services'
import Users from './users'






const DashboardPage = () => {
    const [content, setContent] = useState<JSX.Element>()

    return (
        <div className='dashboardpage'>
            <AsideMenu
                items={[
                    {
                        iconId: '#users',
                        title: 'Участники',
                        onClick: () => setContent(<Users/>)
                    },
                    {
                        iconId: '#book',
                        title: 'Направления',
                        onClick: () => setContent(<h1>Направления</h1>)
                    },
                    {
                        iconId: '#shield',
                        title: 'Администраторы',
                        onClick: () => setContent(<h1>Администраторы</h1>)
                    },
                ]}
            />
            <section className='dashboardpage-content'>
                {content}
            </section>            
        </div>
    )
}

export default DashboardPage