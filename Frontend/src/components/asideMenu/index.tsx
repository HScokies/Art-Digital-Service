import { useState } from 'react'
import './style.scss'
import Logo from 'images/logo.webp'
import Icons from 'images/icons.svg'
import { NavLink } from 'react-router-dom'
import { API } from 'src/services'
import { UseAuth } from 'src/hooks/useAuth'

interface props {
    items: menuElement[]
}

export interface menuElement {
    iconId: string,
    title: string
    to: string
}

const AsideMenu = ({ items }: props) => {
    const { setUserType } = UseAuth();

    const handleLogout = async() => {
        const response = await API.logout();
        if (response.status != 204) return;
        localStorage.removeItem("type");
        setUserType();
    }
    
    return (
        <aside className='aside'>
            <div className="aside_menu">
                <NavLink to='/dashboard'><img className='aside_menu-logo' draggable={false} src={Logo} alt='logo' /></NavLink>
                {
                    items.map((e, i) => {
                        const id = (Date.now() * (i+1)).toString(36);
                        return (
                            <NavLink to={e.to} key={i} className='aside_menu-element' title={e.title} id={id}>
                                <svg>
                                    <use xlinkHref={Icons + e.iconId} />
                                </svg>
                            </NavLink>
                        )
                    }
                    )
                }
                <span className='aside_menu-element' title='Выйти' id='logout' onClick={() => handleLogout()}>
                    <svg>
                        <use xlinkHref={Icons + "#logout"} />
                    </svg>
                </span>
            </div>
        </aside>
    )
}
export default AsideMenu