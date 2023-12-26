import { useEffect, useState } from 'react'
import './style.scss'
import { AsideMenu, DataGridView } from 'src/components'
import { API } from 'src/services'






const DashboardPage = () => {
    const [content, setContent] = useState<JSX.Element>()

    return (
        <div className='dashboardpage'>
            <AsideMenu
                items={[
                    {
                        iconId: '#users',
                        title: 'Участники',
                        onClick: () => setContent(<DataGridView 
                            columns={[
                            {
                                id: "users",
                                title: "Участник",
                            },
                            {
                                id: "userType",
                                title: "Тип",
                            },
                            {
                                id: "cases",
                                title: "Направление",
                                filters: [
                                    {                                    
                                        title: '3D моделирование для компьютерных игр',
                                        name: 'excludeCase',
                                        value: 1,
                                    },
                                    {
                                        title: 'Разработка компьютерных игр',
                                        name: 'excludeCase',
                                        value: 2,
                                    },
                                ]
                            },
                            {
                                id: "score",
                                title: "Балл",
                                filters: [
                                    {
                                        title: 'Есть значение',
                                        name: 'hasScore',
                                        value: false,
                                    },
                                    {
                                        title: 'Нет значения',
                                        name: 'noScore',
                                        value: false,
                                    }
                                ]
                            }
                        ]} source={API.getUsers}
                        />)
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