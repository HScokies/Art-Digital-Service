import './style.scss'
import Logo from 'images/logo.webp'
import Icons from 'images/icons.svg'
import { useContext, useEffect, useState } from 'react'
import { Button, FileInput, Stage } from 'src/components'
import { API } from 'src/services'
import { AuthContext } from 'src/hooks/authContext'
import parse from 'html-react-parser';
import { IProfileData } from 'src/interfaces'

const ProfilePage = () => {
    const { setAuthorized } = useContext(AuthContext)
    const [scrollActive, setScrollActive] = useState(false)  

    const [data, setData] = useState<IProfileData>()
    useEffect(() => {
        setData(API.getProfileData)
    }, [])
    const [menuActive, setMenuActive] = useState(false)
    useEffect(() => {
        const toggleUserMenu = (e: MouseEvent) => {
            const element = e.target as HTMLElement;
            if (!element.classList.contains('usermenu')) {
                setMenuActive(false)
            }
        }
        document.addEventListener('click',  toggleUserMenu)
        return(() => document.removeEventListener('click', toggleUserMenu))
    }, [])

    useEffect(() => {
        const updateScrollState = () => {
            setScrollActive(window.scrollY > 100)            
        }
        document.addEventListener('scroll', updateScrollState)
        return(() => document.removeEventListener('scroll', updateScrollState))
    }, [])
    
    const handleLogout = () => {
        API.logout()
        setAuthorized(false)
    }

    const [buttonActive, setActive] = useState(false)
    const handleFileChange = () => {
        const inputs = document.getElementsByClassName("fileinput_wrapper-field")
        for (let i=0; i<inputs.length; i++){
            const input = inputs[i] as HTMLInputElement
            if (!input.files?.length) return setActive(false)
        }
    setActive(true)
    }

    return (
        <div className='profilepage' >
            <header className='profilepage_header'>
                <div className='profilepage_header_logocontainer'>
                    <img src={Logo} alt='Logo.webp' className='profilepage_header_logocontainer-logo image' />
                    <h1 className='profilepage_header_logocontainer-logo text'>Цифра&bull;Дизайн&bull;Сервис</h1>
                </div>
                <div className='profilepage_header-user_container usermenu' onClick={() => setMenuActive(!menuActive)}>
                    <span className='profilepage_header-user_container-user usermenu'>
                        {data?.user.firstName}
                    </span>
                    <svg className='profilepage_header-user_container-dropdown usermenu'>
                        <use xlinkHref={Icons + "#downarrow"} />
                    </svg>
                    <ul className={`profilepage_header-user_container-menu ${menuActive ? 'active' : ''}`}>
                        <li className="profilepage_header-user_container-menu-item">
                            <span className='profilepage_header-user_container-menu-item-username'>
                                {data?.user.firstName} {data?.user.lastName}
                            </span>
                            <span className='profilepage_header-user_container-menu-item-email'>
                                {data?.user.email}
                            </span>
                        </li>
                        <li className="profilepage_header-user_container-menu-item" onClick={() => handleLogout()}>
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
                    {data?.case.task&&parse(data.case.task)}
                </p>
            </section>
            <section className='profilepage_status'>
                {
                    data?.user.status?
                        <>
                            <h2 className='profilepage_status-title_results'>Результаты</h2>
                            <p className='profilepage_status-text'>{data.user.status.name}</p>
                            {
                                data.user.status.file && 
                                <a onClick={() => API.getFile(data.user.status!.file!)} className='profilepage_status-text link'>
                                Скачать сертификат участника
                                </a>
                            }
                        </>
                        :
                        <>
                            <h2 className='profilepage_status-title'>Отправить задание</h2>
                            <form className='profilepage_status-form' id='task-form'>
                                <FileInput changeHandler={handleFileChange} label='Согласие на обработку персональных данных' required={true} name='consent' accept={['.pdf']} />
                                <FileInput changeHandler={handleFileChange} label='Выполненное задание' name='solution' required={true} accept={['.pdf']} />
                                <Button isActive={buttonActive}>
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
                <iframe className='profilepage_guide-video' frameBorder='0' allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture' allowFullScreen={true} src={data?.case.video}></iframe>
            </section>
            <section className='profilepage_stages'>
                <h2 className='profilepage_stages-title'>
                    Порядок выполнения
                </h2>
                {
                    data?.case.stages.map((e, i) => (
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
                        data?.case.criterias.map((e, i) => (
                            <span key={i} className='profilepage_criterias-container-criteria'>{parse(e)}</span>
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
                            <a href={data?.legal.regulations} className="profilepage_footer_top-container_right-element">
                                Положение об Олимпиаде
                            </a>
                            <a href={data?.legal.privacyPolicy} className="profilepage_footer_top-container_right-element">
                                Политика конфиденциальности
                            </a>
                            <a href={data?.legal.processingConsent} className="profilepage_footer_top-container_right-element">
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
            <span className={`scroll-top ${scrollActive ? 'active' : ''}`} onClick={() => window.scrollTo(0, 0)}>
                <svg>
                    <use xlinkHref={Icons + "#uparrow"} />
                </svg>
            </span>
        </div>
    )
}
export default ProfilePage