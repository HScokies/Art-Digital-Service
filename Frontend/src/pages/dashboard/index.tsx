import { useEffect, useState } from 'react'
import './style.scss'
import { AsideMenu, DataGridView } from 'src/components'
import { orderBy } from 'components/dataGridView/interfaces'






const DashboardPage = () => {
    const [content, setContent] = useState<JSX.Element>()
    const [activeSort, setActiveSort] = useState<orderBy | undefined>()

    return (
        <div className='dashboardpage'>
            <AsideMenu
                items={[
                    {
                        iconId: '#users',
                        title: 'Участники',
                        onClick: () => setContent(<DataGridView columns={[
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
                                options: [
                                    {
                                        id: '3dModeling',
                                        title: '3D моделирование для компьютерных игр',
                                        name: 'excludeCase',
                                        value: 1,
                                        isActive: true,
                                    },
                                    {
                                        id: 'gamedev',
                                        title: 'Разработка компьютерных игр',
                                        name: 'excludeCase',
                                        value: 2,
                                        isActive: true,
                                    },
                                ]
                            },
                            {
                                id: "score",
                                title: "Балл",
                                options: [
                                    {
                                        id: 'hasScore',
                                        title: 'Есть значение',
                                        name: 'hasScore',
                                        value: false,
                                        isActive: true,
                                    },
                                    {
                                        id: 'noScore',
                                        title: 'Нет значения',
                                        name: 'noScore',
                                        value: false,
                                        isActive: true,
                                    }
                                ]
                            }
                        ]
                        }/>)
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