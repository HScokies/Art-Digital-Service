import './style.scss'
import Icons from 'images/icons.svg'
import { useState } from 'react'
import { Button, FileInput } from 'src/components'

const ProfilePage = () => {
    const dbdata = {
        user: "Иванов Иван",
        email: "example@email.com",
        task: `Разработать одностраничный сайт на тильде или 
        при помощи любого бесплатного онлайн конструктора 
        на тему: сайт-визитка компании. 
        По итогу работы подготовить презентацию 
        с рядом обязательных слайдов.`,
        sendedFiles: false,
        statustext: `Ваша заявки принята и находится на рассмотрении.
        Ожидайте результатов!`
    }
    const [menuActive, setMenuActive] = useState(false)
    const handleLogout = () => {
        console.debug("logout")
    }
    const toggleMenu = (e: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
        const element = e.target as Element
        if (!element.classList.contains("profilepage_header-user_icon")) {
            setMenuActive(false)
        }
    }

    return (
        <div className='profilepage' onClick={(e) => toggleMenu(e)}>
            <header className='profilepage_header'>
                <h1 className='profilepage_header-logo'>Цифра&bull;Дизайн&bull;Сервис</h1>
            </header>
            <section className='profilepage_task'>
                <h2 className='profilepage_task-title'>
                    Задание
                </h2>
                <p className='profilepage_task-descr'>
                    {dbdata.task}
                </p>
            </section>
            <section className='profilepage_status'>
                {
                    dbdata.sendedFiles ?
                        <>
                            <h2 className='profilepage_status-title_wait'>Результаты</h2>
                        </>
                        :
                        <>
                            <h2 className='profilepage_status-title'>Отправить задание</h2>
                            <form className='profilepage_status-form' id='task-form'>
                                <FileInput label='Согласие на обработку персональных данных' name='consent' accept={['.pdf']} />
                                <FileInput label='Выполненное задание' name='solution' accept={['.pdf']} />
                                <Button isActive={true}>
                                    Отправить
                                </Button>
                            </form>
                        </>

                }
            </section>
        </div>
    )
}
export default ProfilePage