import './style.scss'
import Logo from 'images/logo.webp'
import Icons from 'images/icons.svg'
import { useEffect, useState } from 'react'
import { CaseData } from 'src/components'
import { API } from 'src/services'
import { IProfileData } from 'src/interfaces'
import { UseAuth } from 'src/hooks/useAuth'


const ProfilePage = () => {
    const [scrollActive, setScrollActive] = useState(false)  
    const {setUserType} = UseAuth()

    const [data, setData] = useState<IProfileData>()
    useEffect(() => {
        const fetch = async() => {
            const response = await API.getProfile()
            if (response.status != 200) return;
            setData(response.data)
        }
        fetch()
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
    
    const handleLogout = async() => {
        await API.logout();
        setUserType(undefined);
    }

    return (
        data&&
        <div className='profilepage' >
            <header className='profilepage_header'>
                <div className='profilepage_header_logocontainer'>
                    <img src={Logo} loading="lazy" alt='Logo.webp' className='profilepage_header_logocontainer-logo image' />
                    <h1 className='profilepage_header_logocontainer-logo text'>Цифра&bull;Дизайн&bull;Сервис</h1>
                </div>
                <div className='profilepage_header-user_container usermenu' onClick={() => setMenuActive(!menuActive)}>
                    <span className='profilepage_header-user_container-user usermenu'>
                        {data.firstName}
                    </span>
                    <svg className='profilepage_header-user_container-dropdown usermenu'>
                        <use xlinkHref={Icons + "#downarrow"} />
                    </svg>
                    <ul className={`profilepage_header-user_container-menu ${menuActive ? 'active' : ''}`}>
                        <li className="profilepage_header-user_container-menu-item">
                            <span className='profilepage_header-user_container-menu-item-username'>
                                {data.firstName} {data.lastName}
                            </span>
                            <span className='profilepage_header-user_container-menu-item-email'>
                                {data.email}
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
            <CaseData userStatus={data.status} caseData={data.case} />
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
                            <a href={`${API.URL}files/legal/regulations.pdf?displayedName=Положение об Олимпиаде`} target='_blank' className="profilepage_footer_top-container_right-element">
                                Положение об Олимпиаде
                            </a>
                            <a href={`${API.URL}files/legal/privacy_policy.pdf?displayedName=Политика конфиденциальности`} target='_blank' className="profilepage_footer_top-container_right-element">
                                Политика конфиденциальности
                            </a>
                            <a href={`${API.URL}files/legal/${data.isAdult?"a_consent.pdf" : "y_consent.pdf"}?displayedName=Согласие на обработку персональных данных`} className="profilepage_footer_top-container_right-element">
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