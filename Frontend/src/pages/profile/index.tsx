import './style.scss'
import Icons from 'images/icons.svg'
import { useState } from 'react'
import { Button, FileInput, Stage } from 'src/components'

const ProfilePage = () => {
    const dbdata = {
        user: "Иванов Иван",
        email: "example@email.com",
        task: `Разработать одностраничный сайт на тильде или 
        при помощи любого бесплатного онлайн конструктора 
        на тему: сайт-визитка компании. 
        По итогу работы подготовить презентацию 
        с рядом обязательных слайдов.`,
        sendedFiles: true,
        statustext: `Ваша заявки принята и находится на рассмотрении.
        Ожидайте результатов!`,
        guideURL: 'https://rutube.ru/play/embed/463d2697d78bef84dc937b06e1a42699',
        stages: [
            'Выбрать компанию, для которой будет разработан сайт (название, сфера деятельности, товары или услуги, информация о команде этой компании; подобрать фото, текст, видео, ссылки)',
            'Создать «Заголовок страницы» (текст, фото)',
            'Создать раздел «Услуги или товары» (текст, фото, кнопки, ссылки)',
            'Создать раздел «Наша команда» (текст, фото, видео, ссылки, кнопки)',
            'Создать раздел «Контакты» (ссылки, текст)',
            `Создать «Подвал (футер* сайта)» (кнопка обратной связи, знак авторского права,иконки социальных сетей)
*Футер сайта (подвал) – это сквозной повторяющийся структурный элемент, расположенный в нижней части страниц сайта`,
            'Содержательно наполнить контентом и оформить все разделы',
            'Дизайн сайта - оформить сайт в едином стиле',
            `Подготовить презентацию
1-й слайд – титульный (название сайта, название конструктора, фамилия, имя, отчество участника)
2-й слайд – обосновать, почему выбрали именно эту компанию (до 5 предложений)
3-й слайд – скрин «Заголовка» сайта
4-й слайд – скрин раздела «Услуги или товары»
5-й слайд – скрин раздела «Наша команда»
6-й слайд – скрин раздела «Контакты»
7-й слайд – скрин «Подвала (футера сайта)»
8-й слайд - ссылка на разработанный и опубликованный сайт `,
            'Отправить заявку на участие в олимпиаде, прикрепив презентацию в формате pdf'
        ]
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
                            <p>%Placeholder%</p>
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
            <section className='profilepage_guide'>
                <h2 className='profilepage_guide-title'>
                    Мастер-класс
                </h2>
                <iframe className='profilepage_guide-video' allowFullScreen={true} src={dbdata.guideURL}></iframe>
            </section>
            <section className='profilepage_stages'>
                <h2 className='profilepage_stages-title'>
                    Порядок выполнения
                </h2>
                {
                    dbdata.stages.map((e, i) => (
                        <Stage key={i} index={i+1} text={e} />
                    ))
                }
            </section>
        </div>
    )
}
export default ProfilePage