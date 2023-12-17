import { useEffect, useState } from 'react'
import './style.scss'
import { AsideMenu } from 'src/components'






const DashboardPage = () => {
    const [content, setContent] = useState<JSX.Element>()
    const renderUsers = () => (
        <>
            <h1>Пользователи</h1>
            <div className='table'>
                <td>test</td>
            </div>
        </>
    )

    return (
        <div className='dashboardpage'>
            <AsideMenu
                items={[
                    {
                        iconId: '#users',
                        title: 'Участники',
                        onClick: () => setContent(renderUsers())
                    }
                ]}
            />
            <section className='dashboardpage-content'>
                {content}
            </section>            
        </div>
    )
}

export default DashboardPage