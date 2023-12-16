import './style.scss'
import Logo from 'images/logo.webp'
import Icons from 'images/icons.svg'
import { useState } from 'react'
import { Button, FileInput, Stage } from 'src/components'

const ProfilePage = () => {
    const dbdata = {
        firstName: "Иван",
        lastName: "Иванов",
        email: "example@email.com",
        task: `Разработать одностраничный сайт на тильде или 
        при помощи любого бесплатного онлайн конструктора 
        на тему: сайт-визитка компании. 
        По итогу работы подготовить презентацию 
        с рядом обязательных слайдов.`,
        sendedFiles: false,
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
        ],
        criterias: [
            "Оформление титульного листа",
            "Обоснованность выбора компании",
            "«Заголовок» сайта",
            "Контент раздела «Услуги или товары»"
        ]
    }
    const [menuActive, setMenuActive] = useState(false)
    const handleLogout = () => {
        console.debug("logout")
    }
    return (
        <div className='profilepage'>
            <header className='profilepage_header'>
                <img src={Logo} alt='Logo.webp' className='profilepage_header-logo image' />
                <h1 className='profilepage_header-logo text'>Цифра&bull;Дизайн&bull;Сервис</h1>
                <div className='profilepage_header-user_container'>
                    <span className='profilepage_header-user_container-user'>
                        {dbdata.firstName}
                    </span>
                    <svg className='profilepage_header-user_container-dropdown'>
                        <use xlinkHref={Icons + "#downarrow"} />
                    </svg>
                    <ul className='profilepage_header-user_container-menu'>
                        <li className="profilepage_header-user_container-menu-item">
                            <span className='profilepage_header-user_container-menu-item-username'>
                                {dbdata.firstName} {dbdata.lastName}
                            </span>
                            <span className='profilepage_header-user_container-menu-item-email'>
                                {dbdata.email}
                            </span>
                        </li>
                        <li className="profilepage_header-user_container-menu-item">
                            <span className='profilepage_header-user_container-menu-item-logout'>
                                Выйти
                            </span>
                        </li>
                    </ul>
                </div>
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
                <iframe className='profilepage_guide-video' frameBorder='0' allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture' allowFullScreen={true} src={dbdata.guideURL}></iframe>
            </section>
            <section className='profilepage_stages'>
                <h2 className='profilepage_stages-title'>
                    Порядок выполнения
                </h2>
                {
                    dbdata.stages.map((e, i) => (
                        <Stage key={i} index={i + 1} text={e} />
                    ))
                }
            </section>
            <section className='profilepage_criterias'>
                <h2 className='profilepage_criterias-title'>
                    Критерии
                </h2>
                <div className='profilepage_criterias-container'>
                    {
                        dbdata.criterias.map((e, i) => (
                            <span key={i} className='profilepage_criterias-container-criteria'>{e}</span>
                        ))
                    }
                </div>
            </section>
            <footer className='profilepage_footer'>
                <section className='profilepage_footer_top'>
                    <h3 className='profilepage_footer_top-title'>
                        Контакты
                    </h3>
                    <div className='profilepage_footer_top-container'>
                        <address className='profilepage_footer_top-container_left'>
                            <a className='profilepage_footer_top-container_left-element' href='tel:+73512161011'>8 (351) 216-10-11</a>
                            <a className='profilepage_footer_top-container_left-element' href='mailto:olymp@midis.ru'>olymp@midis.ru</a>
                            <span className='profilepage_footer_top-container_left-element'>г. Челябинск, ул. Ворошилова, 12</span>
                        </address>
                        <div className='profilepage_footer_top-container_right'>
                            <a href="" className="profilepage_footer_top-container_right-element">
                                Положение об Олимпиаде
                            </a>
                            <a href="" className="profilepage_footer_top-container_right-element">
                                Политика конфиденциальности
                            </a>
                            <a href="" className="profilepage_footer_top-container_right-element">
                                Согласие на обработку персональных данных
                            </a>
                        </div>
                    </div>
                </section>
                <section className='profilepage_footer_bottom'>
                    <a className='profilepage_footer_bottom-copy' href='https://midis.ru/' target='_blank'>&copy;&nbsp;Международный Институт Дизайна и&nbsp;Сервиса.</a>
                    <div className='profilepage_footer_bottom-socials'>
                        <a className='profilepage_footer_bottom-socials-link' href='https://vk.com/midis' target='_blank'>
                            <svg>
                                <use xlinkHref={Icons + '#vk'} />
                            </svg>
                        </a>
                        <a className='profilepage_footer_bottom-socials-link' href='https://t.me/midisru' target='_blank'>
                            <svg>
                                <use xlinkHref={Icons + '#telegram'} />
                            </svg>
                        </a>
                    </div>
                </section>
            </footer>
            <span className={`scroll-top ${window.scrollY > 100 ? 'active' : ''}`} onClick={() => window.scrollTo(0, 0)}>
                <svg>
                    <use xlinkHref={Icons + "#uparrow"} />
                </svg>
            </span>
        </div>
    )
}
export default ProfilePage